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
        private readonly ExportService _exportService;
        private readonly string _testDirectory = "TestCsvDirectory";
        private readonly string _csvFileName = "test—lients.csv";

        public ExportServiceTests()
        {
            _context = new BankSystemDbContext();
            _testDataGenerator = new TestDataGenerator();
            _clientStorage = new ClientStorage(_context);
            _clientService = new ClientService(_clientStorage);
            _exportService = new ExportService(_testDirectory, _csvFileName, _clientService, _clientStorage);
        }

        [Fact]
        public void ExportClientDataInCsvShouldExportClientDataInCsv()
        {
            var clients = new List<Client>
            {
                new Client
                {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AA123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
                },

                new Client
                {
                Name = "Dmitry",
                Surname = "Petrov",
                PassportData = "AB123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
                }
            };

            foreach (var client in clients)
            {
                _clientService.AddClient(client);
            }

            _exportService.ExportClientDataInCsv(clients);

            string filePath = Path.Combine(_testDirectory, _csvFileName);
            Assert.True(File.Exists(filePath));

            var fileContent = File.ReadAllText(filePath);
            Assert.Contains("Alex", fileContent);
            Assert.Contains("Dmitry", fileContent);

            foreach (var client in clients)
            {
                _clientService.DeleteClient(client.Id);
            }
        }

        [Fact]
        public void ImportClientDataFromCsvShouldImportClientDataFromCsv()
        {
            var csvContent = "TelephoneNumber,Id,Name,Surname,PassportData,BirthDate,Age\r\n" +
            "1234567890,4436ffff-b080-427f-a597-65529fb9d6ee,Alex,Ivanov,AA123456789,1999-10-18T16:12:39Z,25\r\n" +
            "1234567890,b1e6bddd-4b36-4a3a-b03d-bd1974df29c6,Dmitry,Petrov,AB123456789,1999-10-18T16:12:39Z,25\r\n";

            string filePath = Path.Combine(_testDirectory, _csvFileName);
            Directory.CreateDirectory(_testDirectory);
            File.WriteAllText(filePath, csvContent);
            
            _exportService.ImportClientDataFromCsv();

            var clients = _context.Clients.ToList();
            Assert.Equal(2, clients.Count);
            Assert.Contains(clients, c => c.PassportData == "AA123456789");
            Assert.Contains(clients, c => c.PassportData == "AB123456789");

            foreach (var client in clients)
            {
                _clientService.DeleteClient(client.Id);
            }
        }

        [Fact]
        public void ImportClientDataFromCsvShouldUpdateExistingClients()
        {
            var client = new Client
            {
                Id = Guid.Parse("14204f92-1ddf-4b90-b8bd-fd3a5fd5809d"),
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AA123456987",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                TelephoneNumber = "1234567890"
            };
            _clientService.AddClient(client);

            var csvContent = "TelephoneNumber,Id,Name,Surname,PassportData,BirthDate,Age\r\n" +
            "1234567890,14204f92-1ddf-4b90-b8bd-fd3a5fd5809d,Alex,Aleksandrov,AA123456789,1999-10-18T16:12:39Z,25\r\n";

            string filePath = Path.Combine(_testDirectory, _csvFileName);
            Directory.CreateDirectory(_testDirectory);
            File.WriteAllText(filePath, csvContent);

            _exportService.ImportClientDataFromCsv();

            var updatedClient = _clientStorage.GetById(client.Id);

            Assert.NotNull(updatedClient);
            Assert.Equal("AA123456789", updatedClient.PassportData);
            Assert.Equal("Aleksandrov", updatedClient.Surname);

            _clientService.DeleteClient(updatedClient.Id);
        }

        [Fact]
        public void ImportClientDataFromCsvShouldThrowFileNotFoundExceptionWhenFileDoesNotExist()
        {
            string filePath = Path.Combine(_testDirectory, _csvFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var exception = Assert.Throws<FileNotFoundException>(() => _exportService.ImportClientDataFromCsv());
            Assert.Equal("‘‡ÈÎ ‰Îˇ ËÏÔÓÚ‡ ÌÂ Ì‡È‰ÂÌ", exception.Message);
        }
    }
}