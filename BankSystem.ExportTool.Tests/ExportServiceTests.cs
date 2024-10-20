using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using ExportTool;

namespace BankSystem.ExportTool.Tests
{
    public class ExportServiceTests
    {
        private readonly BankSystemDbContext _context;
        private readonly ClientService _clientService;
        private readonly ClientStorage _clientStorage;
        private readonly TestDataGenerator _testDataGenerator;
        private readonly ExportService<Client> _exportService;
        private readonly string _testDirectory = "TestCsvDirectory";
        private readonly string _csvFileName = "testClients.csv";

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

            _exportService.ExportDataInCsv(clients, _testDirectory, _csvFileName);

            string filePath = Path.Combine(_testDirectory, _csvFileName);
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

            _exportService.ExportDataInCsv(clients, _testDirectory, _csvFileName);

            string filePath = Path.Combine(_testDirectory, _csvFileName);
            Assert.True(File.Exists(filePath));
            
            _exportService.ImportDataFromCsv(_testDirectory, _csvFileName);

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
            string filePath = Path.Combine(_testDirectory, _csvFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var exception = Assert.Throws<FileNotFoundException>(() => _exportService.ImportDataFromCsv(_testDirectory, _csvFileName));
            Assert.Equal("Файл для импорта не найден", exception.Message);
        }
    }
}