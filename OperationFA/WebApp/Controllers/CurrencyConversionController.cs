using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyConversionController : ControllerBase
    {
        private readonly ICurrencyConversionService _conversionService;
        private readonly ILogger<CurrencyConversionController> _logger;

        public CurrencyConversionController(ICurrencyConversionService conversionService, ILogger<CurrencyConversionController> logger)
        {
            _conversionService = conversionService;
            _logger = logger;
        }

        [HttpPost("currencies/{currencyCode}/conversion")]
        public async Task<IActionResult> PostCurrencyConversion(string currencyCode, [FromBody] CurrencyConversionRequest request)
        {
            try
            {
                var result = await _conversionService.ConvertCurrencyAsync(request);
                if (result == null)
                {
                    return NotFound("Currency conversion data not found.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PostCurrencyConversion");
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
    }
}
