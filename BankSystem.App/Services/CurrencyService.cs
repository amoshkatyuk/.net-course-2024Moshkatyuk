using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace BankSystem.App.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public CurrencyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["CurrencyApi:ApiKey"];
            _baseUrl = "https://www.amdoren.com/api/currency.php";
        }

        public async Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount, CancellationToken cancellationToken) 
        {
            var builder = new StringBuilder(_baseUrl);

            builder.Append($"?api_key={_apiKey}");
            builder.Append($"&from={fromCurrency}");
            builder.Append($"&to={toCurrency}");
            builder.Append($"&amount={amount}");

            var requestUrl = builder.ToString();

            var response = await _httpClient.GetAsync(requestUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonConvert.DeserializeObject<CurrencyApiResponse>(responseContent);

            if (result == null || result.Error != 0)
            {
                throw new CurrencyConversionException("Ошибка при попытке конвертации валюты");
            }

            return result.Amount;
        }
    }
}
