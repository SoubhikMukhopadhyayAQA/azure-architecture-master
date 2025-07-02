namespace DotNetAzureServicesCall.Services.ServiceInterface
{
    public interface IFileShareService
    {
        bool ValidateFileShareExists(string connectionString, string fileShareName);
        Task UploadFileAsync(string connectionString, string fileShareName, string sourceFilePath, string destinationFilePath);
        bool CheckFileExists(string connectionString, string fileShareName, string filePath);
        Task FailedUploadFileAsync(string connectionString, string fileShareName, string sourceFilePath, string destinationFilePath);
        Task DeleteFileIfExists(string connectionString, string fileShareName, string filePath);
    }
}
