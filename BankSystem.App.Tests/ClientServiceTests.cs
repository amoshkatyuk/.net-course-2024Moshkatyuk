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
        public void GetClientByIdShouldReturnClientById() 
        {
            var client = _testDataGenerator.GenerateClient();
            _clientService.AddClient(client);

            var desiredClient = _clientService.GetClientById(client.Id);

            Assert.NotNull(desiredClient);
            Assert.Equal(client.PassportData, desiredClient.PassportData);

            _clientService.DeleteClient(client.Id);
        }

        [Fact]
        public void AddClientShouldAddClient() 
        {
            var client = _testDataGenerator.GenerateClient();
            _clientService.AddClient(client);

            var existingClient = _clientService.GetClientById(client.Id);

            Assert.Equal(existingClient.PassportData, client.PassportData);

            _clientService.DeleteClient(client.Id);
        }

        [Fact]
        public void GetClientsByFilterShouldReturnFilteredClients() 
        {
            var firstClient = _testDataGenerator.GenerateClient();
            var secondClient = _testDataGenerator.GenerateClient();

            _clientService.AddClient(firstClient);
            _clientService.AddClient(secondClient);

            var filteredClients = _clientService.FilterClients(c => c.PassportData == secondClient.PassportData);

            Assert.Single(filteredClients);

            _clientService.DeleteClient(firstClient.Id);
            _clientService.DeleteClient(secondClient.Id);
        }

        [Fact]
        public void UpdateClientShouldUpdateExistingClient() 
        {
            var existingClient = _testDataGenerator.GenerateClient();

            _clientService.AddClient(existingClient);

            existingClient.TelephoneNumber = "37377883636";
            _context.Clients.Update(existingClient);

            var updatedClient = _clientService.GetClientById(existingClient.Id);

            Assert.Equal("37377883636", updatedClient.TelephoneNumber);

            _clientService.DeleteClient(existingClient.Id);
        }

        [Fact]
        public void AddAccountShouldAddAccountToClient() 
        {
            var client = _testDataGenerator.GenerateClient();
            _clientService.AddClient(client);

            var account = _testDataGenerator.GenerateAccount(_context);

            _clientService.AddAdditionalAccount(client.Id, account);

            var updatedClient = _clientService.GetClientById(client.Id);

            Assert.Contains(account, updatedClient.Accounts);

            _clientService.DeleteClient(client.Id);
        }

        [Fact]
        public void DeleteClientsAccountShouldDeleteClientAccount() 
        {
            var client = _testDataGenerator.GenerateClient();
            _clientService.AddClient(client);

            _clientService.DeleteAccount(client.Id, client.Accounts.First().Id);

            Assert.DoesNotContain(client.Accounts.FirstOrDefault(), client.Accounts);

            _clientService.DeleteClient(client.Id);
        }
    }
}
