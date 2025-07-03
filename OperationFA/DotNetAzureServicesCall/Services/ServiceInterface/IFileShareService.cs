namespace DotNetAzureServicesCall.Services.ServiceInterface
{
    public interface IFileShareService
    {
        bool ValidateFileShareExists(string connectionString, string fileShareName);
        Task UploadFileAsync(string connectionString, string fileShareName, string sourceFilePath, string destinationFilePath);
        bool CheckFileExists(string connectionString, string fileShareName, string filePath);
        Task<bool> CheckFileExistsAsync(string connectionString, string fileShareName, string filePath);
        Task FailedUploadFileAsync(string connectionString, string fileShareName, string sourceFilePath, string destinationFilePath);
        Task DeleteFileIfExists(string connectionString, string fileShareName, string filePath);
        Task UploadFileAsync2(string connectionString, string fileShareName, string sourceFilePath, string destinationFilePath);
        Task DeleteFilesWithPrefix(string connectionString, string fileShareName, string filePath);
        Task FailedUploadFileAsync();
        Task DeleteFilesWithDynamicSuffix(string connectionString, string fileShareName, string filePath);
        Task<bool> VerifyXmlInZipRecursiveAsync(string connectionString, string fileShareName, string zipFilePath, string xmlFileName);
        Task DeleteFileIfExistsAsync(string connectionString, string fileShareName, string filePath);
        Task<bool> VerifyXmlFilesInZipAsync(string connectionString, string fileShareName, string zipFilePath, List<string> xmlFileNames);
        Task<bool> VerifyXmlFilesInZipByPrefixAsync(string connectionString, string fileShareName, string destinationPath, string zipFileId, List<string> xmlFileNames);
        Task<bool> CheckFileWithIdInNameExistsAsync(string connectionString, string fileShareName, string directoryName, string idInFileName);
    }
}
