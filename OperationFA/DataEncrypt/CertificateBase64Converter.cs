using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System;

namespace DataEncrypt
{
    public class CertificateBase64Converter
    {
        private readonly ILogger<CertificateBase64Converter> _logger;

        public CertificateBase64Converter(ILogger<CertificateBase64Converter> logger)
        {
            _logger = logger;
        }

        [Function("CertificateBase64Converter")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Loading certificate and converting to base64...");

            string pfxFilePath = @"C:\Users\s.p.mukhopadhyay\source\repos\TestData\Demo\biztalkkeyenciphertest.aqa.org.uk.pfx";
            string pfxPassword = "";

            HttpResponseData response;

            try
            {
                var certificate = new X509Certificate2(pfxFilePath, pfxPassword);
                byte[] certData = certificate.Export(X509ContentType.Cert);
                string base64Cert = Convert.ToBase64String(certData);

                response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteStringAsync(base64Cert, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading certificate or converting to base64.");

                response = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await response.WriteStringAsync($"Error: {ex.Message}", Encoding.UTF8);
            }

            return response;
        }
    }
}