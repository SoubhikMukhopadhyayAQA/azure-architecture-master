using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Net;

namespace FileZipOperation;

public class ZipOps
{
    private readonly ILogger<ZipOps> _logger;

    public ZipOps(ILogger<ZipOps> logger)
    {
        _logger = logger;
    }

    [Function("ZipOps")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        string filePath = query["filePath"];

        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Invalid or missing file path.");
            return badResponse;
        }

        try
        {
            string zipFilePath = Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(filePath)}.zip");

            using (var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/zip");
            response.Headers.Add("Content-Disposition", $"attachment; filename={Path.GetFileName(zipFilePath)}");

            var stream = File.OpenRead(zipFilePath);
            await response.WriteBytesAsync(await File.ReadAllBytesAsync(zipFilePath));
            stream.Close();

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error zipping file");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("An error occurred while zipping the file.");
            return errorResponse;
        }
    }
}