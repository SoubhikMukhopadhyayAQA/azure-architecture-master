using Azure.Storage.Files.Shares;
using DotNetAzureServicesCall.Services.ServiceInterface;

namespace DotNetAzureServicesCall.Services
{
    public class FileShareService : IFileShareService
    {
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
        public bool CheckFileExists(string connectionString, string fileShareName, string filePath)
        {
            ShareFileClient fileClient = new ShareFileClient(connectionString, fileShareName, filePath);
            return fileClient.Exists();
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
    }
}
