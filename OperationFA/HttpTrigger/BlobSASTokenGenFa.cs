using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HttpTrigger;

public class BlobSASTokenGenFa
{
    private readonly ILogger<BlobSASTokenGenFa> _logger;

    public BlobSASTokenGenFa(ILogger<BlobSASTokenGenFa> logger)
    {
        _logger = logger;
    }

    [Function("Function1")]
    public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req, ILogger log)
    {
        string containerName = req.Headers["containerName"];     //Dynamic User Input                                    
        string folderPath = req.Headers["folderPath"];           //Dynamic User Input
        string blobName = req.Headers["blobName"];               //Dynamic User Input
                                                                 //string sasTokenTTL = req.Headers["sasTokenTTL"];         //Dynamic User Input
        string storageAccountName = Environment.GetEnvironmentVariable("storageAccountName");
        string storageAccountKey = Environment.GetEnvironmentVariable("storageAccountKey");

        string fullPath = containerName + folderPath;

        try
        {

            //****************** Set storage account blob client and container function *********************
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                    storageAccountName, storageAccountKey), true);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(fullPath);
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            //****************** Set storage account blob client and container function *********************
            SharedAccessBlobPolicy sasPolicy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,          ///Read only permission set - as per the requirement

                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(5)  ///dyn
            };

            string sasToken = blob.GetSharedAccessSignature(sasPolicy);

            return new OkObjectResult(sasToken);

        }
        catch (Exception e)
        {
            //var error = new { error = "Bad Parameter send" };
            return new ObjectResult(e);
            //{
            //StatusCode = 500
            //};
        }
    }
}