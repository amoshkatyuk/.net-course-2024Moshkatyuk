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
        public async Task GetByIdShouldGetClientById()
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AA123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            await _clientStorage.AddAsync(client);

            var result = await _clientStorage.GetByIdAsync(client.Id);

            Assert.Equal(client, result);

            await _clientStorage.DeleteAsync(client.Id);
        }

        [Fact]
        public async Task AddClientShouldAddClient()
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };

            await _clientStorage.AddAsync(client);

            var result = await _clientStorage.GetByIdAsync(client.Id);

            Assert.Equal("Alex", result.Name);

            await _clientStorage.DeleteAsync(client.Id);
        }

        [Fact]
        public async Task GetClientsByFilterShouldReturnFilteredClients() 
        {
            var firstClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AC123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            await _clientStorage.AddAsync(firstClient);

            var secondClient = new Client
            {
                Name = "Nick",
                Surname = "Ivanov",
                PassportData = "AD123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            await _clientStorage.AddAsync(secondClient);

            var filteredClients = await _clientStorage.GetAsync(c => c.Name == "Nick");

            Assert.Equal(filteredClients.First().Name, secondClient.Name);

            await _clientStorage.DeleteAsync(firstClient.Id);
            await _clientStorage.DeleteAsync(secondClient.Id);
        }

        [Fact]
        public async Task UpdateClientShouldUpdateExistingClient() 
        {
            var existingClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AE123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            await _clientStorage.AddAsync(existingClient);

            existingClient.Surname = "Stepanov";
            await _clientStorage.UpdateAsync(existingClient.Id, existingClient);

            var updatedClient = await _clientStorage.GetByIdAsync(existingClient.Id);

            Assert.Equal("Stepanov", updatedClient.Surname);

            await _clientStorage.DeleteAsync(existingClient.Id);
        }

        [Fact]
        public async Task DeleteClientShouldDeleteExistingClient() 
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AF123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            await _clientStorage.AddAsync(client);

            await _clientStorage.DeleteAsync(client.Id);

            var result = await _clientStorage.GetByIdAsync(client.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAccountShouldAddAccountToClient() 
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

            var account = new Account {Currency = currency, Amount = 1000 };
            
            await _clientStorage.AddAccountAsync(client.Id, account);
            
            var updatedClient = await _clientStorage.GetByIdAsync(client.Id);

            Assert.Contains(account, updatedClient.Accounts);

            await _clientStorage.DeleteAsync(client.Id);
        }

        [Fact]
        public async Task DeleteClientsAccountShouldDeleteClientAccount() 
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
            await _clientStorage.AddAsync(client);

            var currency = new Currency { Type = "USD" };

            var account = new Account {Currency = currency, Amount = 1000 };
            await _clientStorage.AddAccountAsync(client.Id, account);

            await _clientStorage.DeleteAccountAsync(client.Id, account.Id);

            var updatedClient = await _clientStorage.GetByIdAsync(client.Id);

            Assert.DoesNotContain(account, updatedClient.Accounts);

            await _clientStorage.DeleteAsync(client.Id);
        }

        [Fact]
        public async Task GetClientsAverageAgeReturnAverageAge() 
        {
            var firstClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AH123456789",
                BirthDate = DateTime.UtcNow.AddYears(-20),
                TelephoneNumber = "1234567890",
            };
            await _clientStorage.AddAsync(firstClient);

            var secondClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AH123456789",
                BirthDate = DateTime.UtcNow.AddYears(-30),
                TelephoneNumber = "1234567890",
            };
            await _clientStorage.AddAsync(secondClient);

            var averageAge = await _clientStorage.GetAverageAgeAsync();

            Assert.Equal(25, averageAge);

            await _clientStorage.DeleteAsync(firstClient.Id);
            await _clientStorage.DeleteAsync(secondClient.Id);
        }
    }
}