using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using ExportTool;
using Newtonsoft.Json;

namespace BankSystem.ExportTool.Tests
{
    public class ExportServiceTests
    {
        private readonly BankSystemDbContext _context;
        private readonly ClientService _clientService;
        private readonly ClientStorage _clientStorage;
        private readonly TestDataGenerator _testDataGenerator;
        private readonly ExportService<Client> _exportService;
        private readonly string _testCsvDirectory = "TestCsvDirectory";
        private readonly string _csvFileName = "testClients.csv";
        private readonly string _testJsonDirectory = "TestJsonDirectory";
        private readonly string _jsonFileName = "testJsonClients.json";

        public ExportServiceTests()
        {
            _context = new BankSystemDbContext();
            _testDataGenerator = new TestDataGenerator();
            _clientStorage = new ClientStorage(_context);
            _clientService = new ClientService(_clientStorage);
            _exportService = new ExportService<Client>();
        }

        [Fact]
        public void ExportClientDataInCsvShouldExportClientDataInCsv()
        {
            var clients = new List<Client>();
            clients.Add(_testDataGenerator.GenerateClient());
            clients.Add(_testDataGenerator.GenerateClient());

            foreach (var client in clients)
            {
                _clientService.AddClient(client);
            }

            _exportService.ExportDataInCsv(clients, _testCsvDirectory, _csvFileName);

            string filePath = Path.Combine(_testCsvDirectory, _csvFileName);
            Assert.True(File.Exists(filePath));

            var fileContent = File.ReadAllText(filePath);
            Assert.Contains(clients[0].Name, fileContent);
            Assert.Contains(clients[1].Name, fileContent);

            foreach (var client in clients)
            {
                _clientService.DeleteClient(client.Id);
            }
        }

        [Fact]
        public void ImportClientDataFromCsvShouldImportClientDataFromCsv()
        {
            var clients = new List<Client>();
            clients.Add(_testDataGenerator.GenerateClient());
            clients.Add(_testDataGenerator.GenerateClient());

            foreach (var client in clients)
            {
                _clientService.AddClient(client);
            }

            _exportService.ExportDataInCsv(clients, _testCsvDirectory, _csvFileName);

            string filePath = Path.Combine(_testCsvDirectory, _csvFileName);
            Assert.True(File.Exists(filePath));
            
            _exportService.ImportDataFromCsv(_testCsvDirectory, _csvFileName);

            var importedClients = _context.Clients.ToList();

            Assert.Equal(2, clients.Count);
            foreach (var client in clients)
            {
                Assert.Contains(importedClients, c => c.PassportData == client.PassportData);
            }

            foreach (var client in clients)
            {
                _clientService.DeleteClient(client.Id);
            }
        }

        [Fact]
        public void ImportClientDataFromCsvShouldThrowFileNotFoundExceptionWhenFileDoesNotExist()
        {
            string filePath = Path.Combine(_testCsvDirectory, _csvFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var exception = Assert.Throws<FileNotFoundException>(() => _exportService.ImportDataFromCsv(_testCsvDirectory, _csvFileName));
            Assert.Equal("Файл для импорта не найден", exception.Message);
        }

        [Fact]
        public void ExportSerializedDataToFileShouldExportClientDataInJson() 
        {
            var clients = new List<Client>();
            clients.Add(_testDataGenerator.GenerateClient());
            clients.Add(_testDataGenerator.GenerateClient());

            foreach (var client in clients)
            {
                _clientService.AddClient(client);
            }

            _exportService.ExportSerializedDataToFile(clients, _testJsonDirectory, _jsonFileName);

            string filePath = Path.Combine(_testJsonDirectory, _jsonFileName);
            Assert.True(File.Exists(filePath));

            var fileContent = File.ReadAllText(filePath);
            var deserealizedClients = JsonConvert.DeserializeObject<List<Client>>(fileContent);
            Assert.NotNull(deserealizedClients);
            Assert.Equal(2, deserealizedClients.Count);
            Assert.Contains(deserealizedClients, c => c.PassportData == clients[0].PassportData);
            Assert.Contains(deserealizedClients, c => c.PassportData == clients[1].PassportData);

            foreach (var client in clients) 
            {
                _clientService.DeleteClient(client.Id);
            }
        }

        [Fact]
        public void ImportSerializedDataFromFileShouldImportClientDataFromJson()
        {
            var clients = new List<Client>
            {
                _testDataGenerator.GenerateClient(),
                _testDataGenerator.GenerateClient()
            };

            foreach (var client in clients)
            {
                _clientService.AddClient(client);
            }

            _exportService.ExportSerializedDataToFile(clients, _testJsonDirectory, _jsonFileName);

            string filePath = Path.Combine(_testJsonDirectory, _jsonFileName);
            Assert.True(File.Exists(filePath));

            var importedClients = _exportService.ImportSerializedDataFromFile(_testJsonDirectory, _jsonFileName).ToList();

            Assert.Equal(2, importedClients.Count);
            Assert.Contains(importedClients, c => c.PassportData == clients[0].PassportData);
            Assert.Contains(importedClients, c => c.PassportData == clients[1].PassportData);

            foreach (var client in clients)
            {
                _clientService.DeleteClient(client.Id);
            }
        }

        [Fact]
        public void ImportSerializedDataFromFileShouldThrowFileNotFoundExceptionWhenFileDoesNotExist()
        {
            string filePath = Path.Combine(_testJsonDirectory, _jsonFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var exception = Assert.Throws<FileNotFoundException>(() => _exportService.ImportSerializedDataFromFile(_testJsonDirectory, _jsonFileName));
            Assert.Equal("Файл для импорта не найден", exception.Message);
        }
    }
}