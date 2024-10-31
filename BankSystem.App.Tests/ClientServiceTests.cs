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

        public ClientServiceTests()
        {
            _context = new BankSystemDbContext();
            _testDataGenerator = new TestDataGenerator();
            _clientService = new ClientService(new ClientStorage(_context));
        }

        [Fact]
        public async Task GetClientByIdShouldReturnClientById() 
        {
            var client = _testDataGenerator.GenerateClient();
            await _clientService.AddClientAsync(client);

            var desiredClient = await _clientService.GetClientByIdAsync(client.Id);

            Assert.NotNull(desiredClient);
            Assert.Equal(client.PassportData, desiredClient.PassportData);

            await _clientService.DeleteClientAsync(client.Id);
        }

        [Fact]
        public async Task AddClientShouldAddClient() 
        {
            var client = _testDataGenerator.GenerateClient();
            await _clientService.AddClientAsync(client);

            var existingClient = await _clientService.GetClientByIdAsync(client.Id);

            Assert.Equal(existingClient.PassportData, client.PassportData);

            await _clientService.DeleteClientAsync(client.Id);
        }

        [Fact]
        public async Task GetClientsByFilterShouldReturnFilteredClients() 
        {
            var firstClient = _testDataGenerator.GenerateClient();
            var secondClient = _testDataGenerator.GenerateClient();

            await _clientService.AddClientAsync(firstClient);
            await _clientService.AddClientAsync(secondClient);

            var filteredClients = await _clientService.FilterClientsAsync(c => c.PassportData == secondClient.PassportData);

            Assert.Single(filteredClients);

            await _clientService.DeleteClientAsync(firstClient.Id);
            await _clientService.DeleteClientAsync(secondClient.Id);
        }

        [Fact]
        public async Task UpdateClientShouldUpdateExistingClient() 
        {
            var existingClient = _testDataGenerator.GenerateClient();

            await _clientService.AddClientAsync(existingClient);

            existingClient.TelephoneNumber = "37377883636";

            await _clientService.UpdateClientAsync(existingClient);

            var updatedClient = await _clientService.GetClientByIdAsync(existingClient.Id);

            Assert.Equal("37377883636", updatedClient.TelephoneNumber);

            await _clientService.DeleteClientAsync(existingClient.Id);
        }

        [Fact]
        public async Task AddAccountShouldAddAccountToClient() 
        {
            var client = _testDataGenerator.GenerateClient();
            await _clientService.AddClientAsync(client);

            var account = _testDataGenerator.GenerateAccount(_context);

            await _clientService.AddAdditionalAccountAsync(client.Id, account);

            var updatedClient = await _clientService.GetClientByIdAsync(client.Id);

            Assert.Contains(account, updatedClient.Accounts);

            await _clientService.DeleteClientAsync(client.Id);
        }

        [Fact]
        public async Task DeleteClientsAccountShouldDeleteClientAccount() 
        {
            var client = _testDataGenerator.GenerateClient();
            await _clientService.AddClientAsync(client);

            await _clientService.DeleteAccountAsync(client.Id, client.Accounts.First().Id);

            Assert.DoesNotContain(client.Accounts.FirstOrDefault(), client.Accounts);

            await _clientService.DeleteClientAsync(client.Id);
        }

        [Fact]
        public async Task WithdrawFromAccountsAsyncShouldReturnTrueWhenMoneyIsSuccessfullyWithdrawn() 
        {
            var firstClient = _testDataGenerator.GenerateClient();
            var secondClient = _testDataGenerator.GenerateClient();

            await _clientService.AddClientAsync(firstClient);
            await _clientService.AddClientAsync(secondClient);

            firstClient.Accounts.FirstOrDefault().Amount = 5000;
            secondClient.Accounts.FirstOrDefault().Amount = 5000;

            await _clientService.UpdateClientAsync(firstClient);
            await _clientService.UpdateClientAsync(secondClient);

            var withdrawalRequests = new Dictionary<Guid, List<decimal>>
            {
                { firstClient.Id, new List<decimal> { 500m, 1500m } },
                { secondClient.Id, new List<decimal> { 300m, 700m, 1000m } }
            };

            var result = await _clientService.WithdrawFromAccountsAsync(withdrawalRequests);

            Assert.True(result);

            var updatedFirstClient = await _clientService.GetClientByIdAsync(firstClient.Id);
            var updatedSecondClient = await _clientService.GetClientByIdAsync(secondClient.Id);


            await _clientService.DeleteClientAsync(firstClient.Id);
            await _clientService.DeleteClientAsync(secondClient.Id);

        }
    }
}
