using BankSystem.Data.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;
using BankSystem.App.Exceptions;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private EmployeeStorage _employeeStorage;

        public EmployeeService()
        {
            _employeeStorage = new EmployeeStorage();
        }

        public void AddEmployee(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.PassportData))
            {
                throw new NoPassportDataException("Работник не имеет паспортных данных");
            }

            if (employee.Age < 18)
            {
                throw new UnderagePeopleException("Несовершеннолетний работник");
            }

            _employeeStorage.AddEmployee(employee);
        }

        public List<Employee> FilterEmployees(string name = null, string surname = null, string passportData = null, decimal? salary = null, string position = null, string contract = null, DateTime? birthDateFrom = null, DateTime? birthDateTo = null) 
        {
            return _employeeStorage.FilterEmployees(name, surname, passportData, salary, position, contract, birthDateFrom, birthDateTo);
        }
    }
}
