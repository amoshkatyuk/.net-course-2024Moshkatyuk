using BankSystem.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using BankSystem.App.Exceptions;

namespace BankSystem.App.Tests
{
    public class ClientServiceTests
    {
        private ClientService _clientService;
        private ClientStorage _clientStorage;

        public ClientServiceTests() 
        {
            _clientStorage = new ClientStorage();
            _clientService = new ClientService(_clientStorage);
        }

        [Fact]
        public void AddClientShouldAddClientWithDefaultAccount()
        {
            var newClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };

            _clientService.AddClient(newClient);
            var clients = _clientService.FilterClients(passportData: newClient.PassportData);
            var accounts = _clientService.GetAccounts(clients[0]);

            Assert.NotNull(clients);
            Assert.Equal(1, clients?.Count);
            Assert.Equal(newClient.PassportData, clients[0].PassportData);

            Assert.NotNull(accounts);
            Assert.Equal(1, accounts?.Count);
            Assert.Equal("USD", accounts[0].Currency);
            Assert.Equal(0, accounts[0].Amount);
        }

        [Fact]
        public void AddClientShouldThrowUnderageException() 
        {
            var underageClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-15),
                TelephoneNumber = "1234567890"
            };

            var exception = Assert.Throws<UnderagePeopleException>(() => _clientService.AddClient(underageClient));
            Assert.Equal("Несовершеннолетний клиент", exception.Message);
        }

        [Fact]
        public void AddClientShouldThrowNoPassportDataException() 
        {
            var noPassportDataClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };

            var exception = Assert.Throws<NoPassportDataException>(() => _clientService.AddClient(noPassportDataClient));
            Assert.Equal("Клиент не имеет паспортных данных", exception.Message);
        }

        [Fact]
        public void AddDefaultAccountForNewClientShouldAddDollarAccount() 
        {
            var newClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };

            _clientService.AddClient(newClient);

            var accounts = _clientService.GetAccounts(newClient);
            Assert.Contains(accounts, a=> a.Currency == "USD");
        }

        [Fact]
        public void AddAdditionalAccountForClientShouldAddNewAccountToExistingClient() 
        {
            var newClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientService.AddClient(newClient);

            var newAccount = new Account
            {
                Currency = "RUB",
                Amount = 5000
            };
            _clientService.AddAdditionalAccount(newClient, newAccount);

            var accounts = _clientService.GetAccounts(newClient);
            Assert.Contains(accounts, a=> a.Currency == newAccount.Currency && a.Amount == newAccount.Amount);
        }

        [Fact]
        public void UpdateExistingAccountShouldUpdateExistingAccount() 
        {
            var newClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientService.AddClient(newClient);

            var newAccount = new Account
            {
                Currency = "RUB",
                Amount = 5000
            };
            _clientService.AddAdditionalAccount(newClient, newAccount);

            var updatedAccount = new Account
            {
                Currency = "RUB",
                Amount = 10000
            };
            _clientService.UpdateAccount(newClient, updatedAccount);

            var accounts = _clientService.GetAccounts(newClient);
            
            Assert.NotEmpty(accounts);

            var existingAccount = accounts.FirstOrDefault(a => a.Currency == updatedAccount.Currency);
            Assert.NotNull(existingAccount);
            Assert.Equal(updatedAccount.Amount, existingAccount.Amount);

        }

        [Fact]
        public void FilterClientsShoultReturnFilteredClients() 
        {
            var firstClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-40),
                TelephoneNumber = "1234567890"
            };
            _clientService.AddClient(firstClient);

            var secondClient = new Client
            {
                Name = "Nik",
                Surname = "Petrov",
                PassportData = "AB321456789",
                BirthDate = DateTime.Today.AddYears(-60),
                TelephoneNumber = "1234567890"
            };
            _clientService.AddClient(secondClient);

            var thirdClient = new Client
            {
                Name = "Alex",
                Surname = "Petrov",
                PassportData = "AB123654789",
                BirthDate = DateTime.Today.AddYears(-50),
                TelephoneNumber = "1234567890"
            };
            _clientService.AddClient(thirdClient);

            var filteredClients = _clientService.FilterClients("Alex", null, null, null, null);

            Assert.Equal(2, filteredClients.Count);
            Assert.All(filteredClients, c => Assert.Equal("Alex", c.Name));
        }

        [Fact]
        public void UpdateClientShouldReturnUpdatedClient() 
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-40),
                TelephoneNumber = "1234567890"
            };
            _clientService.AddClient(client);

            var updatedClient = new Client
            {
                Name = "Dmitriy",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-40),
                TelephoneNumber = "1234567890"
            };

            _clientService.UpdateClient(updatedClient);

            var clients = _clientService.FilterClients(passportData: updatedClient.PassportData);

            Assert.Equal("Dmitriy", clients[0].Name);
        }

        [Fact]
        public void DeleteClientShouldDeletedClient() 
        {
            var client = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-40),
                TelephoneNumber = "1234567890"
            };
            _clientService.AddClient(client);

            _clientService.DeleteClient(client);

            var clients = _clientService.FilterClients(passportData: client.PassportData);

            Assert.True(clients.Count == 0);
        }
    }
}
