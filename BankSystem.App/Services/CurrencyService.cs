using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

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
            if (string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency) || amount <= 0)
            {
                throw new ArgumentException("Неверные параметры для конвертации валют.");
            }

            var requestUrl = $"{_baseUrl}?api_key={_apiKey}&from={fromCurrency}&to={toCurrency}&amount={amount}";

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
