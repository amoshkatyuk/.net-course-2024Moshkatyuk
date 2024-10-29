using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Tests
{
    public class RateUpdaterTests
    {
        private ClientStorage _clientStorage;
        private BankSystemDbContext _context;
        private TestDataGenerator _testDataGenerator;

        public RateUpdaterTests()
        {
            _context = new BankSystemDbContext();
            _clientStorage = new ClientStorage(_context);
            _testDataGenerator = new TestDataGenerator();
        }

        [Fact]
        public async Task RateUpdaterShouldApplyPercentageToClientAccounts()
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AG123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890",
                Accounts = new List<Account>()
            };
            await _clientStorage.AddAsync(client);

            var currency = new Currency { Type = "RUB" };

            var account = new Account { Currency = currency, Amount = 1000 };

            await _clientStorage.AddAccountAsync(client.Id, account);

            var percentage = 10m;
            var interval = TimeSpan.FromMilliseconds(100);
            var rateUpdater = new RateUpdater(_clientStorage, percentage, interval);

            rateUpdater.Start();
            await Task.Delay(200);

            rateUpdater.Stop();

            var updatedClient = await _clientStorage.GetByIdAsync(client.Id);
            var updatedAccount = updatedClient.Accounts.First();

            
            Assert.Equal(1100, updatedAccount.Amount);

            await _clientStorage.DeleteAsync(client.Id);
        }
    }
}
