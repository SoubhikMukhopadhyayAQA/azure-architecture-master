using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DotNetAzureServicesCall.Services.ServiceInterface;

namespace DotNetAzureServicesCall.Services
{
    public class BlobService : IBlobService
    {
        public bool ValidateBlobContainerExists(string connectionString, string containerName)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                return containerClient.Exists();
            }
            catch (Exception)
            {
                return false;
            }
        }
        /*public (bool, string) ValidateBlobExistsInSubdirectories(string connectionString, string containerName, string directoryPath, string blobPrefix)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                foreach (BlobHierarchyItem item in containerClient.GetBlobsByHierarchy(prefix: directoryPath, delimiter: "/"))
                {
                    if (item.IsPrefix)
                    {
                        foreach (BlobItem blobItem in containerClient.GetBlobs(prefix: item.Prefix + blobPrefix))
                        {
                            string foundBlobName = Path.GetFileName(blobItem.Name);
                            Console.WriteLine($"Found blob: {foundBlobName}");
                            return (true, foundBlobName);
                        }
                    }
                }
                return (false, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating blob existence: {ex.Message}");
                return (false, null);
            }
        }*/
        public (bool, string) ValidateBlobExistsInSubdirectories(string connectionString, string containerName, string directoryPath, string blobPrefix)
        {
            try
            {
                directoryPath = Path.Combine(directoryPath, DateTime.UtcNow.ToString("MM-dd-yyyy"));

                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                foreach (BlobItem blobItem in containerClient.GetBlobs(prefix: directoryPath + "/" + blobPrefix))
                {
                    string foundBlobName = Path.GetFileName(blobItem.Name);
                    Console.WriteLine($"Found blob: {foundBlobName}");
                    return (true, foundBlobName);
                }

                return (false, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating blob existence: {ex.Message}");
                return (false, null);
            }
        }
    }
}
