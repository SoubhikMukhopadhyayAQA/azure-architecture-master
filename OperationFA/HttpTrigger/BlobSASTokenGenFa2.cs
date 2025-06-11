using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace HttpTrigger;

public class BlobSASTokenGenFa2
{
    private readonly ILogger<BlobSASTokenGenFa2> _logger;

    public BlobSASTokenGenFa2(ILogger<BlobSASTokenGenFa2> logger)
    {
        _logger = logger;
    }

    [Function("BlobSASTokenGenFa2")]
    public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req, ILogger log)
    {
        try
        {
            /*string token = req.Headers["containerSasToken"];

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(token);
            log.LogInformation("TEST0.1");
            string containerSasToken = System.Convert.ToBase64String(plainTextBytes);
            log.LogInformation("TEST0.2");
            */


            string containerSasToken = req.Headers["containerSasToken"];
            string blobName = req.Headers["blobName"];
            string containerName = req.Headers["containerName"];
            string blobServiceURL = req.Headers["blobServiceURL"];
            string storageAccountNAME = req.Headers["storageAccountNAME"];
            string storageAccountKEY = req.Headers["storageAccountKEY"];

            // string containerSasToken = "";
            BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerName,
                Resource = "b",
                BlobName = blobName,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                StartsOn = DateTime.UtcNow.AddMinutes(-5)

            };
            blobSasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);
            BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri($"https://{storageAccountNAME}.blob.core.windows.net/{containerName}?{containerSasToken}"));
            string blobSasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(storageAccountNAME, storageAccountKEY)).ToString();


            return new OkObjectResult(blobSasToken);


        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error generating Blob SAS token");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}