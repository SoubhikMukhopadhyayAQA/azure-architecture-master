using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAzureServicesCall.Services.ServiceInterface
{
    public interface IBlobService
    {
        bool ValidateBlobContainerExists(string connectionString, string containerName);
        (bool, string) ValidateBlobExistsInSubdirectories(string connectionString, string containerName, string directoryPath, string blobPrefix);
        Task CheckBlobExistsInSubdirectoriesAndDelete(string connectionString, string containerName, string directoryPath, string blobPrefix);
    }
}
