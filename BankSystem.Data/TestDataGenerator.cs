using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using BankSystem.Data;

namespace BankSystem.App.Services
{
    public class TestDataGenerator
    {
        public List<Client> GenerateClients(int clientsCounter) 
        {
            var faker = new Faker<Client>()
                .RuleFor(c => c.Name, f => f.Name.FirstName())
                .RuleFor(c => c.Surname, f => f.Name.LastName())
                .RuleFor(c => c.PassportData, f => "AB" + f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Age, f => f.Random.Number(18, 99))
                .RuleFor(c => c.TelephoneNumber, f => f.Phone.PhoneNumber("(###) ########"));

            var clients = faker.Generate(clientsCounter);

            return clients;
        }

        public Dictionary<string, Client> GenerateClientsDictionary() 
        {
            var clients = GenerateClients(1000);

            var clientsDictionary = clients.ToDictionary(client => client.TelephoneNumber);

            return clientsDictionary;
        }

        public List<Employee> GenerateEmployees(int employeesCounter) 
        {
            var faker = new Faker<Employee>()
                .RuleFor(e => e.Name, f => f.Name.FirstName())
                .RuleFor(e => e.Surname, f => f.Name.LastName())
                .RuleFor(e => e.PassportData, f => "AB" + f.Random.AlphaNumeric(10))
                .RuleFor(e => e.Age, f => f.Random.Number(18, 99))
                .RuleFor(e => e.Salary, f => f.Finance.Amount(8000, 48000, 2))
                .RuleFor(e => e.Position, f => f.Name.JobTitle())
                .RuleFor(e => e.Contract, f => "Full-time");

            var employees = faker.Generate(employeesCounter);

            return employees;
        }

        public Client GenerateClient()
        {
            var faker = new Faker<Client>()
                .RuleFor(c => c.Name, f => f.Name.FirstName())
                .RuleFor(c => c.Surname, f => f.Name.LastName())
                .RuleFor(c => c.PassportData, f => "AB" + f.Random.AlphaNumeric(9))
                .RuleFor(c => c.Age, f => f.Random.Number(18, 99))
                .RuleFor(c => c.TelephoneNumber, f => f.Phone.PhoneNumber("###########"));

            var client = faker.Generate();

            return client;
        }

        public Account GenerateAccount(BankSystemDbContext context) 
        {
            var faker = new Faker();

            var currencyCode = faker.Finance.Currency().Code;

            var currency = context.Currencies.FirstOrDefault(c => c.Type == currencyCode);

            if (currency == null)
            {
                currency = new Domain.Models.Currency 
                { 
                    Type = currencyCode 
                };

            }
            var account = new Account
            {
                Currency = currency,
                Amount = faker.Finance.Amount(5000, 10000, 2)
            };

            return account;
        }

        public Employee GenerateEmployee()
        {
            var faker = new Faker<Employee>()
                .RuleFor(e => e.Name, f => f.Name.FirstName())
                .RuleFor(e => e.Surname, f => f.Name.LastName())
                .RuleFor(e => e.PassportData, f => "AB" + f.Random.AlphaNumeric(9))
                .RuleFor(e => e.Age, f => f.Random.Number(18, 99))
                .RuleFor(e => e.Salary, f => f.Finance.Amount(8000, 48000, 2))
                .RuleFor(e => e.Position, f => f.Name.JobTitle())
                .RuleFor(e => e.Contract, f => "Full-time");

            var employee = faker.Generate();

            return employee;
        }

    }
}
