using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DataEncrypt
{
    public class Encode
    {
        private readonly ILogger<Encode> _logger;

        public Encode(ILogger<Encode> logger)
        {
            _logger = logger;
        }

        [Function("Encode")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Extracting public key from PFX certificate and encrypting input data...");

            string pfxFilePath = @"C:\Users\s.p.mukhopadhyay\source\repos\TestData\Demo\biztalkkeyenciphertest.aqa.org.uk.pfx";
            string pfxPassword = "";

            string result;

            try
            {
                string inputData = await new StreamReader(req.Body).ReadToEndAsync();
               
                var cert = new X509Certificate2(pfxFilePath, pfxPassword, X509KeyStorageFlags.Exportable);
                using var rsa = cert.GetRSAPublicKey();

                if (rsa == null)
                {
                    throw new Exception("RSA public key not found in certificate.");
                }

                byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
                byte[] encryptedBytes = rsa.Encrypt(inputBytes, RSAEncryptionPadding.Pkcs1);

                string encryptedBase64 = Convert.ToBase64String(encryptedBytes);

                result = $"{encryptedBase64}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error encrypting input data");
                result = $"Error: {ex.Message}";
            }

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteStringAsync(result);
            return response;
        }
    }
}