using System;
using BankSystem.Domain.Models;
using BankSystem.App.Services;

namespace Practice
{
    class Program
    {
        public static Employee UpdateContract(Employee employee)
        {
            employee.Contract = $"Для сотрудника {employee.Name} {employee.Surname}, занимающего должность {employee.Position} заработная плата {employee.Salary + 8000}";
            return employee;
        }

        public static Currency UpdateCurrency(ref Currency currency) 
        {
            currency.Type = "RUB";
            return currency;
        }
        static void Main(string[] args)
        {
            Employee me = new Employee ("Alex", "Moshkatyuk", "AB12345678", "Junior .NET Develover");
            UpdateContract(me);

            Currency usdCurrency = new Currency("USD");
            UpdateCurrency(ref usdCurrency); // без "ref" значение так и останется 100 (ожидалось 200) потому, что передалась бы копия структуры, а не ссылка

            BankServices bankServices = new BankServices();
            Employee owner = new Employee("Alex", "Moshkatyuk", "AB12345678", "Owner");
            int salary = bankServices.CalculateOwnerSalary(1000000, 500000, 2);
            owner.Salary = salary;

            Client client = new Client("Alex", "Moshkatyuk", "AB12345678", "87654321");
            Employee employee = bankServices.ConvertClientToEmployee(client);

            //консольный вывод
            Console.WriteLine($"Updated contract: {me.Contract}");

            Console.WriteLine($"Updated currency: {usdCurrency.Type}");

            Console.WriteLine($"Зарплата для владельца: {owner.Salary}");

            Console.WriteLine($"Employee Name: {employee.Name} {employee.Surname}, Passport: {employee.PassportData}, Position: {employee.Position}");
        }
    }
    
}
