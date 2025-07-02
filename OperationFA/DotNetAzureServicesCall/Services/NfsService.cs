using System.Runtime.InteropServices;
using DotNetAzureServicesCall.Services.ServiceInterface;
using JetBrains.Annotations;

namespace DotNetAzureServicesCall.Services
{
    [UsedImplicitly]
    public class NfsService : INfsService
    {
        private const int ResourceScopeGlobalNetwork = 2;
        private const int ResourceTypeDisk = 1;
        private const int ResourceDisplayTypeShare = 3;
        private const int ErrorLogonFailure = 1326;
        private const int ErrorSessionCredentialConflict = 1219;

        public bool CheckFileExists(string userName, string password, string networkPath, string nfsFilePath, string fileName)
        {
            string fileFullPath = $"{networkPath}{nfsFilePath}{fileName}";
            try
            {
                // ReSharper disable once ConvertToUsingDeclaration
                using (new NetworkConnection(networkPath, userName, password))
                {
                    return File.Exists(fileFullPath);
                }
            }
            catch (Exception)
            {
                // ReSharper disable once EmptyGeneralCatchClause
                return false;
            }
        }
        public async Task DeleteFileFromNfsAsync(string userName, string password, string networkPath, string nfsFilePath, string fileName)
        {
            try
            {
                await Task.Run(() =>
                {
                    // ReSharper disable once ConvertToUsingDeclaration
                    using (new NetworkConnection(networkPath, userName, password))
                    {
                        string fullPath = $"{networkPath}{nfsFilePath}{fileName}";

                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        else
                        {
                            throw new FileNotFoundException($"File not found: {fullPath}");
                        }
                    }
                });
            }
            catch (Exception)
            {
                // ReSharper disable once EmptyGeneralCatchClause
            }
        }
        public bool CheckNetworkPathExists(string userName, string password, string driver, string networkPath)
        {
            try
            {
                using (new NetworkConnection(networkPath, userName, password))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to network path: {ex.Message}");
                return false;
            }
        }
        public async Task UploadFileToNfsAsync(string userName, string password, string driver, string networkPath, string nfsDestinationPath, string sourcePath, string fileName)
        {
            try
            {
                using (new NetworkConnection(networkPath, userName, password))
                {
                    //string destinationFilePath = Path.Combine(networkPath, nfsDestinationPath, fileName);
                    string destinationFilePath = networkPath + nfsDestinationPath + fileName;


                    string destinationDirectory = Path.GetDirectoryName(destinationFilePath);
                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }

                    using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
                    using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                    {
                        await sourceStream.CopyToAsync(destinationStream);
                    }

                    Console.WriteLine($"File '{fileName}' uploaded successfully to '{networkPath}'.");
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Source file not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while uploading file: {ex.Message}");
            }
        }
        public bool CheckFileExistsByPartialName(string userName, string password, string driver, string networkPath, string nfsFilePath, string partialFileName)
        {
            try
            {
                using (var connection = new NetworkConnection(networkPath, userName, password))
                {
                    string searchPath = networkPath + nfsFilePath;

                    if (!Directory.Exists(searchPath))
                    {
                        throw new DirectoryNotFoundException($"Directory '{searchPath}' not found in network path '{networkPath}'.");
                    }

                    var matchingFiles = Directory.GetFiles(searchPath, $"*{partialFileName}*");

                    return matchingFiles.Length > 0;
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while checking file existence: {ex.Message}");
                return false;
            }
        }
        public async Task DeleteFileByPartialNameAsync(string userName, string password, string driver, string networkPath, string nfsFilePath, string partialFileName)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (new NetworkConnection(networkPath, userName, password))
                    {
                        string searchPath = networkPath + nfsFilePath;

                        if (!Directory.Exists(searchPath))
                        {
                            throw new DirectoryNotFoundException($"Directory '{searchPath}' not found in network path '{networkPath}'.");
                        }

                        var matchingFiles = Directory.GetFiles(searchPath, $"*{partialFileName}*");

                        if (matchingFiles.Length == 0)
                        {
                            Console.WriteLine($"No files matching partial name '{partialFileName}' found in network path '{networkPath}'.");
                            return;
                        }

                        foreach (var file in matchingFiles)
                        {
                            File.Delete(file);
                            Console.WriteLine($"File '{file}' deleted successfully from network path '{networkPath}'.");
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting files: {ex.Message}");
            }
        }


        // ReSharper disable once ClassWithVirtualMembersNeverInherited.Local 
        private class NetworkConnection : IDisposable
        {
            // ReSharper disable once FieldCanBeMadeReadOnly.Local
            private string _networkName;
            private bool _disposed;

            public NetworkConnection(string networkName, string userName, string password)
            {
                _networkName = networkName;
                EnsureDisconnected(networkName);

                var netResource = new NetResource
                {
                    Scope = ResourceScopeGlobalNetwork,
                    ResourceType = ResourceTypeDisk,
                    DisplayType = ResourceDisplayTypeShare,
                    RemoteName = networkName
                };

                int result = WNetAddConnection2(netResource, password, userName, 0);

                if (result == ErrorSessionCredentialConflict)
                {
                    EnsureDisconnected(networkName);
                }
                else if (result == ErrorLogonFailure)
                {
                    throw new UnauthorizedAccessException("Invalid credentials for network share.");
                }
                else if (result != 0)
                {
                    throw new InvalidOperationException($"Failed to connect to network share. Error code: {result}");
                }
            }

            private void EnsureDisconnected(string networkName)
            {
                WNetCancelConnection2(networkName, 0, true);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            // ReSharper disable once UnusedParameter.Global
            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (!string.IsNullOrEmpty(_networkName))
                    {
                        EnsureDisconnected(_networkName);
                    }
                    _disposed = true;
                }
            }

            ~NetworkConnection()
            {
                Dispose(false);
            }

            [DllImport("mpr.dll")]
            private static extern int WNetAddConnection2(NetResource netResource, string password, string username, int flags);

            [DllImport("mpr.dll")]
            private static extern int WNetCancelConnection2(string name, int flags, bool force);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        [StructLayout(LayoutKind.Sequential)]
        private class NetResource
        {
            // ReSharper disable NotAccessedField.Local
            // ReSharper disable UnassignedField.Compiler
            public int Scope;
            public int ResourceType;
            public int DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
            // ReSharper restore NotAccessedField.Local
            // ReSharper restore UnassignedField.Compiler
        }
    }
}
