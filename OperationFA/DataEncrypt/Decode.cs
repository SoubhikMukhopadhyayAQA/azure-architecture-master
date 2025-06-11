using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DataEncrypt
{
    public class Decode
    {
        private readonly ILogger<Decode> _logger;

        public Decode(ILogger<Decode> logger)
        {
            _logger = logger;
        }

        [Function("Decode")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Loading certificate and decrypting data...");

            string pfxFilePath = @"C:\Users\s.p.mukhopadhyay\source\repos\TestData\Demo\biztalkkeyenciphertest.aqa.org.uk.pfx";
            string pfxPassword = "";

            string result;

            try
            {
                string encryptedBase64 = await new StreamReader(req.Body).ReadToEndAsync();
                var cert = new X509Certificate2(pfxFilePath, pfxPassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
                using var rsa = cert.GetRSAPrivateKey();

                if (rsa == null)
                {
                    throw new Exception("RSA private key not found in certificate.");
                }

                byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);

                result = Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decrypting input data");
                result = $"Error: {ex.Message}";
            }

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteStringAsync(result);
            return response;
        }
    }
}