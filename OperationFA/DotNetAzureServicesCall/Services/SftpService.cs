using DotNetAzureServicesCall.Services.ServiceInterface;
using Renci.SshNet;

namespace DotNetAzureServicesCall.Services
{
    public class SftpService : ISftpService
    {
        public bool CheckDirectoryExists(string host, int port, string username, string password, string directoryPath)
        {
            using (var client = new SftpClient(host, port, username, password))
            {
                try
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        Console.WriteLine($"Checking existence of directory '{directoryPath}' on the SFTP server.");
                        var attributes = client.GetAttributes(directoryPath);
                        return attributes != null && attributes.IsDirectory;
                    }
                    else
                    {
                        throw new Exception("Failed to connect to the SFTP server.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to check directory: {ex.Message}", ex);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        client.Disconnect();
                    }
                }
            }
        }
        public bool CheckFileExists(string host, int port, string username, string password, string filePath)
        {
            bool fileExists = false;

            using (var client = new SftpClient(host, port, username, password))
            {
                try
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        fileExists = client.Exists(filePath);
                    }
                    else
                    {
                        throw new Exception("Failed to connect to the SFTP server.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking file existence: {ex.Message}");
                }
            }

            return fileExists;
        }
        public void UploadFile(string host, int port, string username, string password, string fileName, string sourceFilePath, string destinationFilePath)
        {
            using (var client = new SftpClient(host, port, username, password))
            {
                try
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        using (var fileStream = new FileStream(sourceFilePath, FileMode.Open))
                        {
                            Console.WriteLine($"Uploading file '{fileName}' from '{sourceFilePath}' to '{destinationFilePath}'");
                            client.UploadFile(fileStream, destinationFilePath);
                        }

                        Console.WriteLine("File upload successful.");
                    }
                    else
                    {
                        throw new Exception("Failed to connect to the SFTP server.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    throw new Exception($"Failed to upload file: {ex.Message}", ex);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        client.Disconnect();
                        Console.WriteLine("Disconnected from the SFTP server.");
                    }
                }
            }
        }
        public void DeleteFile(string host, int port, string username, string password, string filePath)
        {
            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    if (client.Exists(filePath))
                    {
                        client.Delete(filePath);
                    }
                    else
                    {
                        throw new Exception($"File '{filePath}' does not exist.");
                    }
                }
                else
                {
                    throw new Exception("Failed to connect to the SFTP server.");
                }
            }
        }
        public void UploadFileFailed(string host, int port, string username, string password, string fileName, string sourceFilePath, string destinationFilePath)
        {
            using (var client = new SftpClient(host, port, username, password))
            {
                try
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        // Simulate an upload failure by throwing an exception
                        Console.WriteLine($"Simulated failure: Unable to upload file '{fileName}' from '{sourceFilePath}' to '{destinationFilePath}'");
                    }
                    else
                    {
                        throw new Exception("Failed to connect to the SFTP server.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    throw new Exception($"Failed to upload file: {ex.Message}", ex);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        client.Disconnect();
                        Console.WriteLine("Disconnected from the SFTP server.");
                    }
                }
            }
        }
    }
}
