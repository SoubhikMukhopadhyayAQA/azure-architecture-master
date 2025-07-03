using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using DotNetAzureServicesCall.Services.ServiceInterface;
using System.IO.Compression;

namespace DotNetAzureServicesCall.Services
{
    public class FileShareService : IFileShareService
    {
        private static readonly SemaphoreSlim UploadSemaphore = new(10, 10);
        public bool ValidateFileShareExists(string connectionString, string fileShareName)
        {
            try
            {
                ShareClient share = new ShareClient(connectionString, fileShareName);
                return share.Exists();
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task UploadFileAsync(string connectionString, string fileShareName, string sourceFilePath, string destinationFilePath)
        {
            try
            {
                ShareFileClient fileClient = new ShareFileClient(connectionString, fileShareName, destinationFilePath);
                using (FileStream stream = File.OpenRead(sourceFilePath))
                {
                    await fileClient.CreateAsync(stream.Length);
                    await fileClient.UploadAsync(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                throw;
            }
        }
        public async Task UploadFileAsync2(string connectionString, string fileShareName, string sourceFilePath, string destinationFilePath)
        {
            await UploadSemaphore.WaitAsync();
            try
            {
                ShareFileClient fileClient = new ShareFileClient(connectionString, fileShareName, destinationFilePath);
                await using FileStream stream = File.OpenRead(sourceFilePath);
                await fileClient.CreateAsync(stream.Length);
                await fileClient.UploadAsync(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while uploading the file: {ex.Message}");
            }
            finally
            {
                UploadSemaphore.Release();
            }
        }
        public bool CheckFileExists(string connectionString, string fileShareName, string filePath)
        {
            ShareFileClient fileClient = new ShareFileClient(connectionString, fileShareName, filePath);
            return fileClient.Exists();
        }
        public async Task<bool> CheckFileExistsAsync(string connectionString, string fileShareName, string filePath)
        {
            ShareFileClient fileClient = new ShareFileClient(connectionString, fileShareName, filePath);
            return await fileClient.ExistsAsync();
        }
        public async Task FailedUploadFileAsync(string connectionString, string fileShareName, string sourceFilePath, string destinationFilePath)
        {
            try
            {
                ShareFileClient fileClient = new ShareFileClient(connectionString, fileShareName, destinationFilePath);
                using (FileStream stream = File.OpenRead(sourceFilePath))
                {
                    await Task.Delay(100);
                    Console.WriteLine("Failed to upload the file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during file upload: {ex.Message}");
                throw;
            }
        }
        public async Task DeleteFileIfExists(string connectionString, string fileShareName, string filePath)
        {
            ShareFileClient fileClient = new ShareFileClient(connectionString, fileShareName, filePath);
            if (await fileClient.ExistsAsync())
            {
                await fileClient.DeleteAsync();
                Console.WriteLine($"File '{filePath}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"File '{filePath}' does not exist.");
            }
        }
        public async Task DeleteFilesWithPrefix(string connectionString, string fileShareName, string filePath)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);
                string filePrefix = fileName.Substring(0, Math.Min(fileName.Length, 9));
                string directoryPath = Path.GetDirectoryName(filePath)?.Replace('\\', '/');
                directoryPath ??= "";

                ShareDirectoryClient directoryClient = new ShareDirectoryClient(connectionString, fileShareName, directoryPath);

                await foreach (ShareFileItem fileItem in directoryClient.GetFilesAndDirectoriesAsync())
                {
                    if (fileItem.IsDirectory)
                    {
                        continue;
                    }

                    if (fileItem.Name.StartsWith(filePrefix))
                    {
                        ShareFileClient fileClient = directoryClient.GetFileClient(fileItem.Name);

                        if (await fileClient.ExistsAsync())
                        {
                            await fileClient.DeleteAsync();
                            Console.WriteLine($"File '{fileItem.Name}' deleted successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during process: {ex.Message}");
                throw;
            }
        }
        public async Task FailedUploadFileAsync()
        {
            try
            {
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during file upload: {ex.Message}");
                throw;
            }
        }
        public async Task DeleteFilesWithDynamicSuffix(string connectionString, string fileShareName, string filePath)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);

                int intIndex = fileName.IndexOf("_INT", StringComparison.Ordinal);
                string suffixToMatch = intIndex >= 0 ? fileName.Substring(intIndex) : string.Empty;

                string directoryPath = Path.GetDirectoryName(filePath)?.Replace('\\', '/') ?? "";

                ShareDirectoryClient directoryClient = new ShareDirectoryClient(connectionString, fileShareName, directoryPath);

                await foreach (ShareFileItem fileItem in directoryClient.GetFilesAndDirectoriesAsync())
                {
                    if (fileItem.IsDirectory)
                    {
                        continue;
                    }
                    if (fileItem.Name.EndsWith(suffixToMatch))
                    {
                        ShareFileClient fileClient = directoryClient.GetFileClient(fileItem.Name);

                        if (await fileClient.ExistsAsync())
                        {
                            await fileClient.DeleteAsync();
                            Console.WriteLine($"File '{fileItem.Name}' deleted successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during process: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> VerifyXmlInZipRecursiveAsync(string connectionString, string fileShareName, string zipFilePath, string xmlFileName)
        {
            ShareFileClient zipFileClient = new ShareFileClient(connectionString, fileShareName, zipFilePath);

            if (!await zipFileClient.ExistsAsync())
            {
                Console.WriteLine($"Zip file '{zipFilePath}' does not exist.");
                return false;
            }

            await using Stream zipStream = (await zipFileClient.DownloadAsync()).Value.Content;
            using ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);

            foreach (ZipArchiveEntry entry in zipArchive.Entries)
            {
                if (entry.FullName.EndsWith(xmlFileName, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"XML file '{xmlFileName}' found in the zip file.");
                    return true;
                }
            }

            Console.WriteLine($"XML file '{xmlFileName}' not found in the zip file.");
            return false;
        }
        public async Task DeleteFileIfExistsAsync(string connectionString, string fileShareName, string filePath)
        {
            ShareFileClient fileClient = new ShareFileClient(connectionString, fileShareName, filePath);
            if (await fileClient.ExistsAsync())
            {
                await fileClient.DeleteAsync();
                Console.WriteLine($"File '{filePath}' deleted successfully.");
            }
            else
            {
                Console.WriteLine($"File '{filePath}' does not exist.");
            }
        }
        public async Task<bool> VerifyXmlFilesInZipAsync(string connectionString, string fileShareName, string zipFilePath, List<string> xmlFileNames)
        {
            ShareFileClient zipFileClient = new ShareFileClient(connectionString, fileShareName, zipFilePath);

            if (!await zipFileClient.ExistsAsync())
            {
                Console.WriteLine($"Zip file '{zipFilePath}' does not exist.");
                return false;
            }

            await using Stream zipStream = (await zipFileClient.DownloadAsync()).Value.Content;
            using ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);

            var zipFileNames = zipArchive.Entries.Select(entry => entry.FullName).ToList();
            foreach (var xmlFileName in xmlFileNames)
            {
                if (!zipFileNames.Any(name => name.EndsWith(xmlFileName, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"XML file '{xmlFileName}' not found in the zip file.");
                    return false;
                }
            }

            return true;
        }
        public async Task<bool> VerifyXmlFilesInZipByPrefixAsync(string connectionString, string fileShareName, string destinationPath, string zipFileId, List<string> xmlFileNames)
        {
            ShareDirectoryClient directoryClient = new ShareDirectoryClient(connectionString, fileShareName, destinationPath);

            if (!await directoryClient.ExistsAsync())
            {
                Console.WriteLine($"Directory '{destinationPath}' does not exist.");
                return false;
            }

            string matchingZipFileName = null;
            await foreach (ShareFileItem fileItem in directoryClient.GetFilesAndDirectoriesAsync())
            {
                if (!fileItem.IsDirectory && fileItem.Name.Contains(zipFileId, StringComparison.OrdinalIgnoreCase) && fileItem.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    matchingZipFileName = fileItem.Name;
                    break;
                }
            }

            if (matchingZipFileName == null)
            {
                Console.WriteLine($"No zip file with prefix '{zipFileId}' was found in the directory.");
                return false;
            }

            string zipFilePath = Path.Combine(destinationPath, matchingZipFileName);
            ShareFileClient zipFileClient = new ShareFileClient(connectionString, fileShareName, zipFilePath);

            if (!await zipFileClient.ExistsAsync())
            {
                Console.WriteLine($"Zip file '{zipFilePath}' does not exist.");
                return false;
            }

            await using Stream zipStream = (await zipFileClient.DownloadAsync()).Value.Content;
            using ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);

            var zipFileNames = zipArchive.Entries.Select(entry => entry.FullName).ToList();
            foreach (var xmlFileName in xmlFileNames)
            {
                if (!zipFileNames.Any(name => name.EndsWith(xmlFileName, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"XML file '{xmlFileName}' not found in the zip file.");
                    return false;
                }
            }

            return true;
        }
        public async Task<bool> CheckFileWithIdInNameExistsAsync(string connectionString, string fileShareName, string directoryName, string idInFileName)
        {
            ShareDirectoryClient directoryClient = new ShareDirectoryClient(connectionString, fileShareName, directoryName);

            if (!await directoryClient.ExistsAsync())
            {
                return false;
            }

            await foreach (ShareFileItem item in directoryClient.GetFilesAndDirectoriesAsync())
            {
                if (!item.IsDirectory && item.Name.Contains(idInFileName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
