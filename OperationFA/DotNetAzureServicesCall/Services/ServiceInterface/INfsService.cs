using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAzureServicesCall.Services.ServiceInterface
{
    public interface INfsService
    {
        bool CheckFileExists(string userName, string password, string networkPath, string nfsFilePath, string fileName);
        Task DeleteFileFromNfsAsync(string userName, string password, string networkPath, string nfsFilePath, string fileName);
        bool CheckNetworkPathExists(string userName, string password, string driver, string networkPath);
        Task UploadFileToNfsAsync(string userName, string password, string driver, string networkPath, string nfsDestinationPath, string sourcePath, string fileName);
        bool CheckFileExistsByPartialName(string userName, string password, string driver, string networkPath, string nfsFilePath, string partialFileName);
        Task DeleteFileByPartialNameAsync(string userName, string password, string driver, string networkPath, string nfsFilePath, string partialFileName);
    }
}
