using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
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
            [FromQuery] CurrencyConversionRequest request,
            CancellationToken cancellationToken) 
        {
            var convertedAmount = await _currencyService.ConvertCurrencyAsync(request.FromCurrency, request.ToCurrency, request.Amount, cancellationToken);

            return Ok(new 
            {
                FromCurrency = request.FromCurrency,
                ToCurrency = request.ToCurrency,
                OriginalAmount = request.Amount,
                ConvertedAmount = convertedAmount
            });
        }
    }
}
