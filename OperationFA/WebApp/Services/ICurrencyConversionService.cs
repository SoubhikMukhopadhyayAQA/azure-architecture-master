using WebApp.Models;

namespace WebApp.Services
{
    public interface ICurrencyConversionService
    {
        Task<CurrencyConversionResponse?> ConvertCurrencyAsync(CurrencyConversionRequest request);
    }
}
