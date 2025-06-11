namespace WebApp.Models
{
    public class CurrencyConversionResponse
    {
        public string? Description { get; set; }
        public string? FromCurrencyCode { get; set; }
        public string? ToCurrencyCode { get; set; }
        public double? SourceAmount { get; set; }
        public double? ConvertedAmount { get; set; }
    }
}
