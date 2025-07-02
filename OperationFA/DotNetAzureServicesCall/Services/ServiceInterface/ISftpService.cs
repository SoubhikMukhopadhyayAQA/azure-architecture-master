namespace DotNetAzureServicesCall.Services.ServiceInterface
{
    public interface ISftpService
    {
        bool CheckDirectoryExists(string host, int port, string username, string password, string directoryPath);
        bool CheckFileExists(string host, int port, string username, string password, string filePath);
        void UploadFile(string host, int port, string username, string password, string fileName, string sourceFilePath, string destinationFilePath);
        void DeleteFile(string host, int port, string username, string password, string filePath);
        void UploadFileFailed(string host, int port, string username, string password, string fileName, string sourceFilePath, string destinationFilePath);
    }
}
