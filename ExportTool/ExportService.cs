using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using CsvHelper;
using System.Globalization;
using Newtonsoft.Json;

namespace ExportTool
{
    public class ExportService<T> where T: class
    {

        public void ExportDataInCsv(IEnumerable<T> entities, string pathToDirectory, string csvFileName) 
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);
            if (!dirInfo.Exists) 
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(pathToDirectory, csvFileName);

            using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate)) 
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream)) 
                {
                    using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture)) 
                    {
                        csvWriter.WriteRecords(entities);
                    }
                }
            }
        }

        public IEnumerable<T> ImportDataFromCsv(string pathToDirectory, string csvFileName) 
        {
            string fullpath = Path.Combine(pathToDirectory, csvFileName);

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
                        return csvReader.GetRecords<T>().ToList();
                    }
                }
            }
        }

        public void ExportSerializedDataToFile(IEnumerable<T> entities, string pathToDirectory, string jsonFileName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(pathToDirectory, jsonFileName);

            string jsonContent = JsonConvert.SerializeObject(entities, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            File.WriteAllText(fullPath, jsonContent);
        }

        public void ExportSerializedDataToFile(T entity, string pathToDirectory, string jsonFileName) 
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);
            if (!dirInfo.Exists) 
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(pathToDirectory, jsonFileName);

            string jsonContent = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            File.WriteAllText(fullPath, jsonContent);
        }

        public IEnumerable<T> ImportSerializedDataFromFile(string pathToDirectory, string jsonFileName) 
        {
            string fullPath = Path.Combine(pathToDirectory, jsonFileName);

            if (!File.Exists(fullPath)) 
            {
                throw new FileNotFoundException("Файл для импорта не найден");
            }

            string jsonContent = File.ReadAllText(fullPath);

            if (string.IsNullOrWhiteSpace(jsonContent)) 
            {
                throw new InvalidOperationException("Файл пуст или содержит некорректные данные");
            }

            try
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonContent);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Ошибка десериализации данных", ex);
            }
        }
    }
}
