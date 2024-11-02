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
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public ClientStorageTests() 
        {
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=BankSystemDb;Username=postgres;Password=admin")
                .Options;

            _context = new BankSystemDbContext(options);
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
            await _clientStorage.AddAsync(client, _cancellationToken);

            var result = await _clientStorage.GetByIdAsync(client.Id, _cancellationToken);

            Assert.Equal(client, result);

            await _clientStorage.DeleteAsync(client.Id, _cancellationToken);
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

            await _clientStorage.AddAsync(client, _cancellationToken);

            var result = await _clientStorage.GetByIdAsync(client.Id, _cancellationToken);

            Assert.Equal("Alex", result.Name);

            await _clientStorage.DeleteAsync(client.Id, _cancellationToken);
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
            await _clientStorage.AddAsync(firstClient, _cancellationToken);

            var secondClient = new Client
            {
                Name = "Nick",
                Surname = "Ivanov",
                PassportData = "AD123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            await _clientStorage.AddAsync(secondClient, _cancellationToken);

            var filteredClients = await _clientStorage.GetAsync(c => c.Name == "Nick", _cancellationToken);

            Assert.Equal(filteredClients.First().Name, secondClient.Name);

            await _clientStorage.DeleteAsync(firstClient.Id, _cancellationToken);
            await _clientStorage.DeleteAsync(secondClient.Id, _cancellationToken);
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
            await _clientStorage.AddAsync(existingClient, _cancellationToken);

            existingClient.Surname = "Stepanov";
            await _clientStorage.UpdateAsync(existingClient.Id, existingClient, _cancellationToken);

            var updatedClient = await _clientStorage.GetByIdAsync(existingClient.Id, _cancellationToken);

            Assert.Equal("Stepanov", updatedClient.Surname);

            await _clientStorage.DeleteAsync(existingClient.Id, _cancellationToken);
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
            await _clientStorage.AddAsync(client, _cancellationToken);

            await _clientStorage.DeleteAsync(client.Id, _cancellationToken);

            var result = await _clientStorage.GetByIdAsync(client.Id, _cancellationToken);

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
            await _clientStorage.AddAsync(client, _cancellationToken);

            var currency = new Currency { Type = "RUB" };

            var account = new Account {Currency = currency, Amount = 1000 };
            
            await _clientStorage.AddAccountAsync(client.Id, account, _cancellationToken);
            
            var updatedClient = await _clientStorage.GetByIdAsync(client.Id, _cancellationToken);

            Assert.Contains(account, updatedClient.Accounts);

            await _clientStorage.DeleteAsync(client.Id, _cancellationToken);
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
            await _clientStorage.AddAsync(client, _cancellationToken);

            var currency = new Currency { Type = "USD" };

            var account = new Account {Currency = currency, Amount = 1000 };
            await _clientStorage.AddAccountAsync(client.Id, account, _cancellationToken);

            await _clientStorage.DeleteAccountAsync(client.Id, account.Id, _cancellationToken);

            var updatedClient = await _clientStorage.GetByIdAsync(client.Id, _cancellationToken);

            Assert.DoesNotContain(account, updatedClient.Accounts);

            await _clientStorage.DeleteAsync(client.Id, _cancellationToken);
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
            await _clientStorage.AddAsync(firstClient, _cancellationToken);

            var secondClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AH123456789",
                BirthDate = DateTime.UtcNow.AddYears(-30),
                TelephoneNumber = "1234567890",
            };
            await _clientStorage.AddAsync(secondClient, _cancellationToken);

            var averageAge = await _clientStorage.GetAverageAgeAsync(_cancellationToken);

            Assert.Equal(25, averageAge);

            await _clientStorage.DeleteAsync(firstClient.Id, _cancellationToken);
            await _clientStorage.DeleteAsync(secondClient.Id, _cancellationToken);
        }
    }
}