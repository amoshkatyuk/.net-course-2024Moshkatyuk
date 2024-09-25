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
