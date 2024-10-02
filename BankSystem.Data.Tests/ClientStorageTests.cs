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
            var clients = testDataGenerator.GenerateClients(1000);
            _clientStorage.AddClients(clients);

            Assert.True(clients.Count == 1000);
        }

        [Fact]
        public void GetYoungestClientShouldReturnYoungestClient() 
        {
            var clients = testDataGenerator.GenerateClients(1000);

            _clientStorage.AddClients(clients);

            var existingClient = clients.First();

            var testClient = new Client
            {
                Name = existingClient.Name,
                Surname = existingClient.Surname,
                PassportData = existingClient.PassportData,
                Age = 16,
                TelephoneNumber = existingClient.TelephoneNumber
            };

            _clientStorage.AddClient(testClient);

            var youngestClient = _clientStorage.GetYoungestClient();

            Assert.Equal(testClient.Name, youngestClient.Name);
        }

        [Fact]
        public void GetOldestClientShouldReturnOldestClient() 
        {
            var clients = testDataGenerator.GenerateClients(1000);

            _clientStorage.AddClients(clients);

            var existingClient = clients.First();

            var testClient = new Client
            {
                Name = existingClient.Name,
                Surname = existingClient.Surname,
                PassportData = existingClient.PassportData,
                Age = 100,
                TelephoneNumber = existingClient.TelephoneNumber
            };

            _clientStorage.AddClient(testClient);

            var oldestClient = _clientStorage.GetOldestClient();

            Assert.Equal(testClient.Name, oldestClient.Name);
        }

        [Fact]
        public void GetClientsAverageAgeReturnAverageAge() 
        {
            var firstTestClient = new Client
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                Age = 20,
                TelephoneNumber = "37377777777"
            };

            var secondTestClient = new Client
            {
                Name = "Igor",
                Surname = "Petrov",
                PassportData = "AB987654321",
                Age = 42,
                TelephoneNumber = "37377888888"
            };

            _clientStorage.AddClient(firstTestClient);
            _clientStorage.AddClient(secondTestClient);

            var clientsAverageAge = _clientStorage.GetClientsAverageAge();

            Assert.Equal(31, clientsAverageAge);
        }
    }
}