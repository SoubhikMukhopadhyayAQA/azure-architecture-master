using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System;

namespace HttpTrigger
{
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
                // Read necessary info from headers
                string blobName = req.Headers["blobName"];
                string containerName = req.Headers["containerName"];
                string blobServiceURL = req.Headers["blobServiceURL"];

                if (string.IsNullOrEmpty(blobName) || string.IsNullOrEmpty(containerName) || string.IsNullOrEmpty(blobServiceURL))
                {
                    return new BadRequestObjectResult("Missing required header(s): blobName, containerName, or blobServiceURL.");
                }

                // Read storage account credentials from environment variables
                string storageAccountNAME = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_NAME");
                string storageAccountKEY = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_KEY");

                if (string.IsNullOrEmpty(storageAccountNAME) || string.IsNullOrEmpty(storageAccountKEY))
                {
                    log.LogError("Storage account credentials are not configured in environment variables.");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                log.LogInformation("Creating BlobServiceClient...");

                var credential = new Azure.Storage.StorageSharedKeyCredential(storageAccountNAME, storageAccountKEY);
                BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(blobServiceURL), credential);

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                log.LogInformation("Building SAS token...");

                BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerClient.Name,
                    BlobName = blobName,
                    Resource = "b",  // blob
                    StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                };

                blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

                string blobSasToken = blobSasBuilder.ToSasQueryParameters(credential).ToString();

                log.LogInformation("SAS token generated successfully.");

                return new OkObjectResult(blobSasToken);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error generating Blob SAS token");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}