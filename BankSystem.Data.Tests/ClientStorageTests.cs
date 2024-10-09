using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Tests
{
    public class ClientStorageTests
    {
        private ClientStorage _clientStorage;
        TestDataGenerator testDataGenerator = new TestDataGenerator();

        public ClientStorageTests() 
        {
            _clientStorage = new ClientStorage();
        }

        [Fact]
        public void AddClientShouldAddClient()
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };

            _clientStorage.Add(client);

            var clients = _clientStorage.GetClients();

            Assert.Equal(1, clients.Count);

            var result = _clientStorage.ClientExists(client);
            Assert.True(result);
        }

        [Fact]
        public void AddAccountToClientShouldAddNewAccount() 
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(client);

            var account = testDataGenerator.GenerateAccount();
            _clientStorage.AddAccount(client, account);


            Assert.Contains(account, _clientStorage.GetAccounts(client));
        }

        [Fact]
        public void UpdateClientAccountShouldUpdateExistingAccount()
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(client);

            var accounts = testDataGenerator.GenerateAccount();

            _clientStorage.AddAccount(client, accounts);

            var updatedAccount = new Account { Currency = accounts.Currency, Amount = 5000 };
            _clientStorage.UpdateAccount(client, updatedAccount);

            var existingAccount = _clientStorage.GetAccounts(client).First(a => a.Currency == updatedAccount.Currency);
            Assert.Equal(5000, existingAccount.Amount);
        }

        [Fact]
        public void FilterClientsShouldReturnCorrectResults()
        {
            var firstClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB321456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(firstClient);

            var secondClient = new Client
            {
                Name = "Nik",
                Surname = "Petrov",
                PassportData = "AB123654789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(secondClient);

            var thirdClient = new Client
            {
                Name = "Alex",
                Surname = "Petrov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(thirdClient);

            var filteredClients = _clientStorage.Get(c => c.Name == "Alex" && c.Surname == "Petrov");

            Assert.Equal(1, filteredClients.Count);
            Assert.Equal("Alex", filteredClients.First().Name);
            Assert.Equal("Petrov", filteredClients.First().Surname);
        }

        [Fact]
        public void GetYoungestClientShouldReturnYoungestClient() 
        {
            var firstClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AA123456789",
                BirthDate = DateTime.Today.AddYears(-20),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(firstClient);

            var secondClient = new Client
            {
                Name = "Nik",
                Surname = "Petrov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(secondClient);

            var thirdClient = new Client
            {
                Name = "Alex",
                Surname = "Petrov",
                PassportData = "AC123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(thirdClient);

            var youngestClient = _clientStorage.GetYoungestClient();

            Assert.Equal(firstClient.Name, youngestClient.Name);
        }

        [Fact]
        public void GetOldestClientShouldReturnOldestClient() 
        {
            var firstClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AA123456789",
                BirthDate = DateTime.Today.AddYears(-20),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(firstClient);

            var secondClient = new Client
            {
                Name = "Nik",
                Surname = "Petrov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(secondClient);

            var thirdClient = new Client
            {
                Name = "Alex",
                Surname = "Petrov",
                PassportData = "AC123456789",
                BirthDate = DateTime.Today.AddYears(-28),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(thirdClient);

            var oldestClient = _clientStorage.GetOldestClient();

            Assert.Equal(thirdClient.Name, oldestClient.Name);
        }

        [Fact]
        public void GetClientsAverageAgeReturnAverageAge() 
        {
            var firstClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AA123456789",
                BirthDate = DateTime.Today.AddYears(-40),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(firstClient);

            var secondClient = new Client
            {
                Name = "Nik",
                Surname = "Petrov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-60),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(secondClient);

            var thirdClient = new Client
            {
                Name = "Alex",
                Surname = "Petrov",
                PassportData = "AC123456789",
                BirthDate = DateTime.Today.AddYears(-50),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(thirdClient);

            var averageAge = _clientStorage.GetClientsAverageAge();

            Assert.Equal(50, averageAge);
        }
    }
}