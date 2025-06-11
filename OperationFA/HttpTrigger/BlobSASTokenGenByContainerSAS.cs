using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace HttpTrigger;

public class BlobSASTokenGenByContainerSAS
{
    private readonly ILogger<BlobSASTokenGenByContainerSAS> _logger;

    public BlobSASTokenGenByContainerSAS(ILogger<BlobSASTokenGenByContainerSAS> logger)
    {
        _logger = logger;
    }

    [Function("BlobSASTokenGenByContainerSAS")]
    public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req, ILogger log)
    {
        try
        {
            string token = req.Headers["containerSasToken"];

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(token);
            log.LogInformation("TEST0.1");
            string containerSasToken = System.Convert.ToBase64String(plainTextBytes);
            log.LogInformation("TEST0.2");

            string blobName = req.Headers["blobName"];
            string containerName = req.Headers["containerName"];
            string blobServiceURL = req.Headers["blobServiceURL"];
            string storageAccountNAME = req.Headers["storageAccountNAME"];
            string storageAccountKEY = req.Headers["storageAccountKEY"];

            log.LogInformation("TEST1");

            BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(blobServiceURL), new Azure.Storage.StorageSharedKeyCredential(storageAccountNAME, storageAccountKEY));
            //BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=sgspocstorage;AccountKey=6kcVOp1dac2/KDO5gqZdzmkcbf++RpGxS9ylQGIMeOk7974aNcXUYctzT/OCzvhagYHdo2F0udS2+AStEIFZ2g==;EndpointSuffix=core.windows.net");

            log.LogInformation("TEST2");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            log.LogInformation("TEST3");
            BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerClient.Name,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTime.UtcNow.AddMinutes(-5),
                ExpiresOn = DateTime.UtcNow.AddHours(1)
            };
            log.LogInformation("TEST4");
            blobSasBuilder.SetPermissions(BlobSasPermissions.Read);
            log.LogInformation("TEST5");
            string blobSasToken = blobSasBuilder.ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(storageAccountNAME, containerSasToken)).ToString();
            log.LogInformation("TEST6");
            return new OkObjectResult(blobSasToken);



        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error generating Blob SAS token");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}