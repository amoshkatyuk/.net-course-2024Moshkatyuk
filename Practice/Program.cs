using System;
using BankSystem.Domain.Models;
using BankSystem.App.Services;
using System.Diagnostics;
using System.Linq;

namespace Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var testDataGenerator = new TestDataGenerator();
            var clients = testDataGenerator.GenerateClients(100000); // сгенерировал больше тысячи, т.к. в таком случае время поиска будет отличаться от нуля
            var clientsDictionary = testDataGenerator.GenerateClientsDictionary();
            var employees = testDataGenerator.GenerateEmployees(50);
            Stopwatch stopwatch = new Stopwatch();

            string searchPhoneNumber = clients[80000].TelephoneNumber;
            stopwatch.Start();
            var foundClientInList = clients.FirstOrDefault(c => c.TelephoneNumber == searchPhoneNumber);
            stopwatch.Stop();
            Console.WriteLine($"Время поиска клиента в списке: {stopwatch.ElapsedMilliseconds} мс");

            stopwatch.Restart();
            var foundClientInDict = clientsDictionary.TryGetValue(searchPhoneNumber, out var client) ? client : null;
            stopwatch.Stop();
            Console.WriteLine($"Время поиска клиента в словаре: {stopwatch.ElapsedMilliseconds} мс");

            var selectedClients = clients.Where(c => c.Age < 20).ToList(); // получаем выборку(список), но чтобы не засорять консоль вывел только количество
            Console.WriteLine($"Клиентов младше 20 лет: {selectedClients.Count}");

            var employeeWithMinSalary = employees.OrderBy(e => e.Salary).FirstOrDefault(); 
            Console.WriteLine($"Сотрудник с минимальной зарплатой: {employeeWithMinSalary?.Surname} {employeeWithMinSalary?.Name} (з.п.: {employeeWithMinSalary?.Salary})");

            stopwatch.Restart();
            var lastClientInDictionary = clientsDictionary.LastOrDefault(c => c.Value.TelephoneNumber == searchPhoneNumber); 
            stopwatch.Stop();
            Console.WriteLine($"Время поиска последнего клиента с LastOrDefault: {stopwatch.ElapsedMilliseconds} мс");

            stopwatch.Restart();
            var clientInDict = clientsDictionary.TryGetValue(searchPhoneNumber, out var clientFound) ? clientFound : null;
            stopwatch.Stop();
            Console.WriteLine($"Время поиска клиента в словаре по ключу: {stopwatch.ElapsedMilliseconds} мс");
        }
    }
    
}
