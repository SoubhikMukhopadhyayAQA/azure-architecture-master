namespace WebApp.Models
{
    public class CurrencyConversionRequest
    {
        public string? FromCurrency { get; set; }
        public string? ToCurrency { get; set; }
        public double? SourceAmount { get; set; }
    }
}
