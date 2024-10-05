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
        public void AddClientsShouldAddClients()
        {
            var clients = testDataGenerator.GenerateClientWithManyAccountsDictionary();
            _clientStorage.AddClients(clients);

            Assert.True(clients.Count == _clientStorage.GetClients().Count);
        }

        [Fact]
        public void ClientExistsShouldReturnTrueIfClientExists()
        {
            var clients = testDataGenerator.GenerateClients(5);
            var client = clients.First();

            _clientStorage.AddClients(new Dictionary<Client, List<Account>> { { client, testDataGenerator.GenerateAccounts(2) } });

            var result = _clientStorage.ClientExists(client);
            Assert.True(result);
        }

        [Fact]
        public void AddAccountToClientShouldAddNewAccount() 
        {
            var clients = testDataGenerator.GenerateClients(5);
            var client = clients.First();
            var account = testDataGenerator.GenerateAccounts(1);

            _clientStorage.AddClients(new Dictionary<Client, List<Account>> { { client, account } });

            var newAccount = testDataGenerator.GenerateAccount();

            _clientStorage.AddAccountToClient(client, newAccount);

            Assert.Contains(newAccount, _clientStorage.GetAccounts(client));
        }

        [Fact]
        public void UpdateClientAccountShouldUpdateExistingAccount()
        {
            var clients = testDataGenerator.GenerateClients(1);
            var client = clients.First();
            var accounts = testDataGenerator.GenerateAccounts(1);
            _clientStorage.AddClients(new Dictionary<Client, List<Account>> { { client, accounts } });

            var updatedAccount = new Account { Currency = accounts.First().Currency, Amount = 5000 };
            _clientStorage.UpdateClientAccount(client, updatedAccount);

            var existingAccount = _clientStorage.GetAccounts(client).First(a => a.Currency == updatedAccount.Currency);
            Assert.Equal(5000, existingAccount.Amount);
        }

        [Fact]
        public void FilterClientsShouldReturnCorrectResults()
        {
            var clients = testDataGenerator.GenerateClients(3);

            clients[0].Name = "John";
            clients[0].Surname = "Doe";
            clients[0].BirthDate = DateTime.Today.AddYears(-25);

            clients[1].Name = "Jane";
            clients[1].Surname = "Smith";
            clients[1].BirthDate = DateTime.Today.AddYears(-30);

            clients[2].Name = "John";
            clients[2].Surname = "Smith";
            clients[2].BirthDate = DateTime.Today.AddYears(-35);

            _clientStorage.AddClients(clients.ToDictionary(c => c, c => testDataGenerator.GenerateAccounts(1)));

            var filteredClients = _clientStorage.FilterClients("John", "Smith", null, null, null, null);

            Assert.Equal(1, filteredClients.Count);
            Assert.Equal("John", filteredClients.First().Name);
            Assert.Equal("Smith", filteredClients.First().Surname);
        }

        [Fact]
        public void GetYoungestClientShouldReturnYoungestClient() 
        {
            var clients = testDataGenerator.GenerateClients(3);

            clients[0].BirthDate = DateTime.Today.AddYears(-18);
            clients[1].BirthDate = DateTime.Today.AddYears(-50);
            clients[2].BirthDate = DateTime.Today.AddYears(-30);

            _clientStorage.AddClients(clients.ToDictionary(c => c, c => testDataGenerator.GenerateAccounts(1)));

            var youngestClient = _clientStorage.GetYoungestClient();

            Assert.Equal(clients[0].Name, youngestClient.Name);
        }

        [Fact]
        public void GetOldestClientShouldReturnOldestClient() 
        {
            var clients = testDataGenerator.GenerateClients(3);

            clients[0].BirthDate = DateTime.Today.AddYears(-18);
            clients[1].BirthDate = DateTime.Today.AddYears(-50);
            clients[2].BirthDate = DateTime.Today.AddYears(-30);

            _clientStorage.AddClients(clients.ToDictionary(c => c, c => testDataGenerator.GenerateAccounts(1)));

            var oldestClient = _clientStorage.GetOldestClient();

            Assert.Equal(clients[1].Name, oldestClient.Name);
        }

        [Fact]
        public void GetClientsAverageAgeReturnAverageAge() 
        {
            var clients = testDataGenerator.GenerateClients(2);

            clients[0].BirthDate = DateTime.Today.AddYears(-20);
            clients[1].BirthDate = DateTime.Today.AddYears(-40);

            _clientStorage.AddClients(clients.ToDictionary(c => c, c => testDataGenerator.GenerateAccounts(1)));

            var averageAge = _clientStorage.GetClientsAverageAge();

            Assert.Equal(30, averageAge);
        }
    }
}