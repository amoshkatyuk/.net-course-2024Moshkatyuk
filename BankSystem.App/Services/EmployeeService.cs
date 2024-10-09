using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeStorage _employeeStorage;

        public EmployeeService(IEmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
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

            _employeeStorage.Add(employee);
        }

        public void DeleteEmployee(Employee employee) 
        {
            if (!_employeeStorage.EmployeeExists(employee)) 
            {
                throw new EntityNotFoundException("Работник не найден");
            }

            _employeeStorage.Delete(employee);
        }

        public void UpdateEmployee(Employee employee) 
        {
            if (!_employeeStorage.EmployeeExists(employee))
            {
                throw new EntityNotFoundException("Работник не найден");
            }

            _employeeStorage.Update(employee);
        }

        public List<Employee> FilterEmployees(Func<Employee, bool> filter) 
        {
            return _employeeStorage.Get(filter);
        }
    }
}
