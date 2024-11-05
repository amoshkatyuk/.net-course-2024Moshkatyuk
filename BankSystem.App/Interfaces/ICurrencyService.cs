using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface ICurrencyService
    {
        Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount, CancellationToken cancellationToken);
    }
}
