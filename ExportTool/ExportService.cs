using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using CsvHelper;
using System.Globalization;

namespace ExportTool
{
    public class ExportService
    {
        private string _pathToDicrectory { get; set; }
        private string _csvFileName { get; set; }

        private readonly ClientService _clientService;
        private readonly ClientStorage _clientStorage;

        public ExportService(string pathToDirectory, string csvFileName, ClientService clientService, ClientStorage clientStorage)
        {
            _pathToDicrectory = pathToDirectory;
            _csvFileName = csvFileName;
            _clientService = clientService;
            _clientStorage = clientStorage;
        }

        public void ExportClientDataInCsv(List<Client> clients) 
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDicrectory);
            if (!dirInfo.Exists) 
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(_pathToDicrectory, _csvFileName);

            using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate)) 
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream)) 
                {
                    using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture)) 
                    {
                        csvWriter.WriteRecords(clients);

                        csvWriter.Flush();
                    }
                }
            }
        }

        public void ImportClientDataFromCsv() 
        {
            string fullpath = Path.Combine(_pathToDicrectory, _csvFileName);

            if (!File.Exists(fullpath))
            {
                throw new FileNotFoundException("Файл для импорта не найден");
            }   

            using (FileStream fileStream = new FileStream(fullpath, FileMode.Open)) 
            {
                using (StreamReader streamReader = new StreamReader(fileStream)) 
                {
                    using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture)) 
                    {
                        var clients = csvReader.GetRecords<Client>().ToList();

                        foreach (var client in clients)
                        {
                            client.BirthDate = DateTime.SpecifyKind(client.BirthDate, DateTimeKind.Utc);

                            var existingClient = _clientStorage.GetById(client.Id); // использую метод из Storage, т.к. в Serivce у нас логика выброса исключения

                            if (existingClient != null)
                            {
                                _clientStorage.Update(client.Id, client);
                            }
                            else
                            {
                                _clientService.AddClient(client);
                            }
                        }
                    }
                }
            }
        }
    }
}
