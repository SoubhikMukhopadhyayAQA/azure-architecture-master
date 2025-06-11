using WebApp.Models;
using Oracle.ManagedDataAccess.Client;

namespace WebApp.Services
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        private readonly ILogger<CurrencyConversionService> _logger;
        private readonly string _connectionString;

        public CurrencyConversionService(ILogger<CurrencyConversionService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("OracleDb")!;
        }

        public async Task<CurrencyConversionResponse?> ConvertCurrencyAsync(CurrencyConversionRequest request)
        {
            try
            {
                using var connection = new OracleConnection(_connectionString);
                await connection.OpenAsync();

                string sql = "SELECT conversion_rate FROM conversion_table WHERE from_currency = :fromCurrency AND to_currency = :toCurrency";
                using var command = new OracleCommand(sql, connection);
                command.Parameters.Add(new OracleParameter("fromCurrency", request.FromCurrency));
                command.Parameters.Add(new OracleParameter("toCurrency", request.ToCurrency));

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    double conversionRate = reader.GetDouble(0);
                    double convertedAmount = (conversionRate * request.SourceAmount.GetValueOrDefault());

                    return new CurrencyConversionResponse
                    {
                        Description = $"1 {request.FromCurrency} = {conversionRate} {request.ToCurrency}",
                        FromCurrencyCode = request.FromCurrency,
                        ToCurrencyCode = request.ToCurrency,
                        SourceAmount = request.SourceAmount,
                        ConvertedAmount = convertedAmount
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Currency conversion failed.");
                throw;
            }
        }
    }
}
