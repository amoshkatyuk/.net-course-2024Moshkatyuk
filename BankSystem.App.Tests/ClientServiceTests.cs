using BankSystem.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using BankSystem.App.Exceptions;
using BankSystem.Data;
using Microsoft.EntityFrameworkCore;
using BankSystem.App.Interfaces;

namespace BankSystem.App.Tests
{
    public class ClientServiceTests
    {
        private readonly BankSystemDbContext _context;
        private readonly ClientService _clientService;
        private readonly TestDataGenerator _testDataGenerator;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public ClientServiceTests()
        {
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=BankSystemDb;Username=postgres;Password=admin")
                .Options;

            _context = new BankSystemDbContext(options);
            _testDataGenerator = new TestDataGenerator();
            _clientService = new ClientService(new ClientStorage(_context));
        }

        [Fact]
        public async Task GetClientByIdShouldReturnClientById() 
        {
            var client = _testDataGenerator.GenerateClient();
            await _clientService.AddClientAsync(client, _cancellationToken);

            var desiredClient = await _clientService.GetClientByIdAsync(client.Id, _cancellationToken);

            Assert.NotNull(desiredClient);
            Assert.Equal(client.PassportData, desiredClient.PassportData);

            await _clientService.DeleteClientAsync(client.Id, _cancellationToken);
        }

        [Fact]
        public async Task AddClientShouldAddClient() 
        {
            var client = _testDataGenerator.GenerateClient();
            await _clientService.AddClientAsync(client, _cancellationToken);

            var existingClient = await _clientService.GetClientByIdAsync(client.Id, _cancellationToken);

            Assert.Equal(existingClient.PassportData, client.PassportData);

            await _clientService.DeleteClientAsync(client.Id, _cancellationToken);
        }

        [Fact]
        public async Task GetClientsByFilterShouldReturnFilteredClients() 
        {
            var firstClient = _testDataGenerator.GenerateClient();
            var secondClient = _testDataGenerator.GenerateClient();

            await _clientService.AddClientAsync(firstClient, _cancellationToken);
            await _clientService.AddClientAsync(secondClient, _cancellationToken);

            var filteredClients = await _clientService.FilterClientsAsync(c => c.PassportData == secondClient.PassportData, _cancellationToken);

            Assert.Single(filteredClients);

            await _clientService.DeleteClientAsync(firstClient.Id, _cancellationToken);
            await _clientService.DeleteClientAsync(secondClient.Id, _cancellationToken);
        }

        [Fact]
        public async Task UpdateClientShouldUpdateExistingClient() 
        {
            var existingClient = _testDataGenerator.GenerateClient();

            await _clientService.AddClientAsync(existingClient, _cancellationToken);

            existingClient.TelephoneNumber = "37377883636";

            await _clientService.UpdateClientAsync(existingClient, _cancellationToken);

            var updatedClient = await _clientService.GetClientByIdAsync(existingClient.Id, _cancellationToken);

            Assert.Equal("37377883636", updatedClient.TelephoneNumber);

            await _clientService.DeleteClientAsync(existingClient.Id, _cancellationToken);
        }

        [Fact]
        public async Task AddAccountShouldAddAccountToClient() 
        {
            var client = _testDataGenerator.GenerateClient();
            await _clientService.AddClientAsync(client, _cancellationToken);

            var account = _testDataGenerator.GenerateAccount(_context);

            await _clientService.AddAdditionalAccountAsync(client.Id, account, _cancellationToken);

            var updatedClient = await _clientService.GetClientByIdAsync(client.Id, _cancellationToken);

            Assert.Contains(account, updatedClient.Accounts);

            await _clientService.DeleteClientAsync(client.Id, _cancellationToken);
        }

        [Fact]
        public async Task DeleteClientsAccountShouldDeleteClientAccount() 
        {
            var client = _testDataGenerator.GenerateClient();
            await _clientService.AddClientAsync(client, _cancellationToken);

            await _clientService.DeleteAccountAsync(client.Id, client.Accounts.First().Id, _cancellationToken);

            Assert.DoesNotContain(client.Accounts.FirstOrDefault(), client.Accounts);

            await _clientService.DeleteClientAsync(client.Id, _cancellationToken);
        }

        [Fact]
        public async Task WithdrawFromAccountsAsyncShouldReturnTrueWhenMoneyIsSuccessfullyWithdrawn() 
        {
            var firstClient = _testDataGenerator.GenerateClient();
            var secondClient = _testDataGenerator.GenerateClient();

            await _clientService.AddClientAsync(firstClient, _cancellationToken);
            await _clientService.AddClientAsync(secondClient, _cancellationToken);

            firstClient.Accounts.FirstOrDefault().Amount = 5000;
            secondClient.Accounts.FirstOrDefault().Amount = 5000;

            await _clientService.UpdateClientAsync(firstClient, _cancellationToken);
            await _clientService.UpdateClientAsync(secondClient, _cancellationToken);

            var withdrawalRequests = new Dictionary<Guid, List<decimal>>
            {
                { firstClient.Id, new List<decimal> { 500m, 1500m } },
                { secondClient.Id, new List<decimal> { 300m, 700m, 1000m } }
            };

            var result = await _clientService.WithdrawFromAccountsAsync(withdrawalRequests, _cancellationToken);

            Assert.True(result);

            var updatedFirstClient = await _clientService.GetClientByIdAsync(firstClient.Id, _cancellationToken);
            var updatedSecondClient = await _clientService.GetClientByIdAsync(secondClient.Id, _cancellationToken);


            await _clientService.DeleteClientAsync(firstClient.Id, _cancellationToken);
            await _clientService.DeleteClientAsync(secondClient.Id, _cancellationToken);

        }
    }
}
