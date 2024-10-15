using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Tests
{
    public class ClientStorageTests
    {
        private ClientStorage _clientStorage;
        private BankSystemDbContext _context;

        public ClientStorageTests() 
        {
            _context = new BankSystemDbContext();
            _clientStorage = new ClientStorage(_context);
        }

        [Fact]
        public void GetByIdShouldGetClientById()
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AA123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(client);

            var result = _clientStorage.GetById(client.Id);

            Assert.Equal(client, result);

            _clientStorage.Delete(client.Id);
        }

        [Fact]
        public void AddClientShouldAddClient()
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };

            _clientStorage.Add(client);

            var result = _clientStorage.GetById(client.Id);

            Assert.Equal("Alex", result.Name);

            _clientStorage.Delete(client.Id);
        }

        [Fact]
        public void GetClientsByFilterShouldReturnFilteredClients() 
        {
            var firstClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AC123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(firstClient);

            var secondClient = new Client
            {
                Name = "Nick",
                Surname = "Ivanov",
                PassportData = "AD123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(secondClient);

            var filteredClients = _clientStorage.Get(c => c.Name == "Nick");

            Assert.Equal(filteredClients.First().Name, secondClient.Name);

            _clientStorage.Delete(firstClient.Id);
            _clientStorage.Delete(secondClient.Id);
        }

        [Fact]
        public void UpdateClientShouldUpdateExistingClient() 
        {
            var existingClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AE123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(existingClient);

            existingClient.Surname = "Stepanov";
            _clientStorage.Update(existingClient);

            var updatedClient = _clientStorage.GetById(existingClient.Id);

            Assert.Equal("Stepanov", updatedClient.Surname);

            _clientStorage.Delete(existingClient.Id);
        }

        [Fact]
        public void DeleteClientShouldDeleteExistingClient() 
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AF123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientStorage.Add(client);

            _clientStorage.Delete(client.Id);

            var result = _clientStorage.GetById(client.Id);

            Assert.Null(result);
        }

        [Fact]
        public void AddAccountShouldAddAccountToClient() 
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
            _clientStorage.Add(client);

            var currency = new Currency { Type = "RUB" };

            var account = new Account {Currency = currency, Amount = 1000 };
            
            _clientStorage.AddAccount(client.Id, account);
            
            var updatedClient = _clientStorage.GetById(client.Id);

            Assert.Contains(account, updatedClient.Accounts);

            _clientStorage.Delete(client.Id);
        }

        [Fact]
        public void DeleteClientsAccountShouldDeleteClientAccount() 
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AH123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890",
                Accounts = new List<Account>()
            };
            _clientStorage.Add(client);

            var currency = new Currency { Type = "USD" };

            var account = new Account {Currency = currency, Amount = 1000 };
            _clientStorage.AddAccount(client.Id, account);

            _clientStorage.DeleteAccount(client.Id, account.Id);

            var updatedClient = _clientStorage.GetById(client.Id);

            Assert.DoesNotContain(account, updatedClient.Accounts);

            _clientStorage.Delete(client.Id);
        }
    }
}