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
        public int CalculateOwnerSalary(Employee employee, int bankProfit, int bankExpenses, int countOfOwners) 
        {
            employee.Salary = (bankProfit - bankExpenses) / countOfOwners;

            return employee.Salary;
        }

        public Employee ConvertClientToEmployee(Client client) // не можем сделать Employee employee = (Employee)client т.к. эти классы уже наследуются от Person
        {
            Employee newEmployee = new Employee(
                client.Name,
                client.Surname,
                client.PassportData,
                "No position"
                );
            return newEmployee;
        }
    }
}
