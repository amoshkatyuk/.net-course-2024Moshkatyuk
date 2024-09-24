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
            Faker faker = new Faker();

            List<Client> clients = new List<Client>();

            for (int i = 0; i < clientsCounter; i++) 
            {
                var client = new Client
                    (
                        faker.Name.FirstName(),
                        faker.Name.LastName(),
                        "AB" + faker.Random.AlphaNumeric(10),
                        faker.Random.Number(18, 99),
                        faker.Finance.Account(),
                        $"373{faker.Random.Number(10000000, 99999999)}"
                    );

                clients.Add(client);
            }
            return clients;
        }

        public Dictionary<string, Client> GenerateClientsDictionary() 
        {
            var clients = GenerateClients(1000);

            var clientsDictionary = new Dictionary<string, Client>();

            foreach (var client in clients) 
            {
                clientsDictionary[client.TelephoneNumber] = client;
            }
            return clientsDictionary;
        }

        public List<Employee> GenerateEmployees(int employeesCounter) 
        {
            Faker faker = new Faker();

            List<Employee> employees = new List<Employee>();

            for (int i = 0; i < employeesCounter; i++)
            {
                var employee = new Employee
                    (
                        faker.Name.FirstName(),
                        faker.Name.LastName(),
                        "AB" + faker.Random.AlphaNumeric(10),
                        faker.Random.Number(18, 99),
                        faker.Finance.Amount(8000, 48000 , 2),
                        faker.Name.JobTitle().ToString()
                    );

                employees.Add(employee);
            }
            return employees;
        }           
    }
}
