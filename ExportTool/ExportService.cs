using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using CsvHelper;
using System.Globalization;

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
    }
}
