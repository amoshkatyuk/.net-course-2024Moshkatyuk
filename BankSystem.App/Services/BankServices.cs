using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class BankServices
    {
        private List<Person> _blackList = new List<Person>();

        public void AddBonus(Person person) 
        {
            if (person is Employee employee)
            {
                employee.Salary += 3000;
            }
            else if (person is Client client)
            {
                Console.WriteLine($"Бонус клиенту {client.Name}{client.Surname} добавлен");
            }
            else 
            {
                throw new InvalidOperationException("Неизвестное лицо");
            }
        }

        public void AddToBlackList<T>(T person) where T : Person 
        {
            if (!IsPersonInBlackList(person)) 
            {
                _blackList.Add(person);
            }
        }

        public bool IsPersonInBlackList<T>(T person) where T : Person 
        {
            return _blackList.Contains(person);
        }

        public int CalculateOwnerSalary(int bankProfit, int bankExpenses, int countOfOwners) 
        {
            return (bankProfit - bankExpenses) / countOfOwners; ;
        }

        public Employee ConvertClientToEmployee(Client client, decimal salary = 0, string position = "No position") // не можем сделать Employee employee = (Employee)client т.к. эти классы уже наследуются от Person
        {
            Employee newEmployee = new Employee();
            newEmployee.Name = client.Name;
            newEmployee.Surname = client.Surname;
            newEmployee.PassportData = client.PassportData;
            newEmployee.Age = client.Age;
            newEmployee.Salary = salary;
            newEmployee.Position = position;
            newEmployee.Contract = "Контракт не составлен";

            return newEmployee;
        }
    }
}
