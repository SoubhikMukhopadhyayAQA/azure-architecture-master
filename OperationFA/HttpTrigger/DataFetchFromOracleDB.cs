using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace HttpTrigger
{
    public static class DataFetchFromOracleDB
    {
        [Function("DataFetchFromOracleDB")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            FunctionContext context)
        {
            var logger = context.GetLogger("DataFetchFromOracleDB");

            string connectionString = Environment.GetEnvironmentVariable("OracleConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                logger.LogError("OracleConnectionString environment variable is not set.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(requestBody))
                return new BadRequestObjectResult("Request body is empty.");

            string currencyCode;
            try
            {
                var data = System.Text.Json.JsonSerializer.Deserialize<RequestPayload>(requestBody);
                currencyCode = data?.CurrencyCode;
            }
            catch (System.Text.Json.JsonException)
            {
                return new BadRequestObjectResult("Invalid JSON format in request body.");
            }

            if (string.IsNullOrWhiteSpace(currencyCode))
                return new BadRequestObjectResult("Currency code is missing in the request payload.");

            try
            {
                await using var connection = new OracleConnection(connectionString);
                await connection.OpenAsync();

                await using var cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "xxc_currency_converter.get_currency_precision";
                cmd.BindByName = true;

                cmd.Parameters.Add("p_currency_code", OracleDbType.Varchar2, ParameterDirection.Input).Value = currencyCode;
                cmd.Parameters.Add("p_precision", OracleDbType.Int64, ParameterDirection.Output);
                cmd.Parameters.Add("p_extended_precision", OracleDbType.Int64, ParameterDirection.Output);

                await cmd.ExecuteNonQueryAsync();

                var precision = ((OracleDecimal)cmd.Parameters["p_precision"].Value).ToInt64();
                var extendedPrecision = ((OracleDecimal)cmd.Parameters["p_extended_precision"].Value).ToInt64();

                var response = new
                {
                    Precision = precision,
                    ExtendedPrecision = extendedPrecision
                };

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching data from Oracle DB.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private class RequestPayload
        {
            public string CurrencyCode { get; set; }
        }
    }
}