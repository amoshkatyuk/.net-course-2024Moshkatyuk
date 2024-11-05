using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency(
            [FromQuery] string fromCurrency,
            [FromQuery] string toCurrency,
            [FromQuery] decimal amount,
            CancellationToken cancellationToken) 
        {
            var convertedAmount = await _currencyService.ConvertCurrencyAsync(fromCurrency, toCurrency, amount, cancellationToken);

            return Ok(new 
            {
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                OriginalAmount = amount,
                ConvertedAmount = convertedAmount
            });
        }
    }
}
