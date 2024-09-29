using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;

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
                .RuleFor(c => c.TelephoneNumber, f => f.Phone.PhoneNumber("(###) #####"));

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
                .RuleFor(e => e.Contract, f => "Контракт не составлен");

            var employees = faker.Generate(employeesCounter);

            return employees;
        }

        public Dictionary<Client, Account> GenerateClientAccountDictionary()
        {
            var clients = GenerateClients(3);

            var clientAccountDictionary = new Dictionary<Client, Account>();

            foreach (var client in clients)
            {
                var account = GenerateAccount();
                clientAccountDictionary[client] = account;
            }

            return clientAccountDictionary;
        }

        public Dictionary<Client, List<Account>> GenerateClientWithManyAccountsDictionary() 
        {
            var clients = GenerateClients(3);

            var clientAccountsDictionary = new Dictionary<Client, List<Account>>();

            foreach (var client in clients)
            {
                var account = GenerateAccounts(2);
                clientAccountsDictionary[client] = account;
            }

            return clientAccountsDictionary;
        }

        public Account GenerateAccount() 
        {
            var faker = new Faker<Account>()
                .RuleFor(a => a.Currency, f => f.Finance.Currency().Code)
                .RuleFor(a => a.Amount, f => f.Finance.Amount(0, 10000, 2));

            var account = faker.Generate();

            return account;
        }

        public List<Account> GenerateAccounts(int accountCount) 
        {
            var faker = new Faker<Account>()
                .RuleFor(a => a.Currency, f => f.Finance.Currency().Code)
                .RuleFor(a => a.Amount, f => f.Finance.Amount(0, 10000, 2));

            var accounts = faker.Generate(accountCount);

            return accounts;
        }
    }
}
