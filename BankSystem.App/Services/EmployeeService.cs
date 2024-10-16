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
        private readonly IStorage<Employee> _employeeStorage;

        public EmployeeService(IStorage<Employee> employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public Employee GetEmployeeById(Guid employeeId)
        {
            var employee = _employeeStorage.GetById(employeeId);

            if (employee == null)
            {
                throw new EntityNotFoundException("Искомый работник не найден");
            }

            return employee;
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

        public List<Employee> FilterEmployees(Func<Employee, bool> filter)
        {
            return _employeeStorage.Get(filter);
        }

        public void UpdateEmployee(Employee employee)
        {
            if (_employeeStorage.GetById(employee.Id) == null)
            {
                throw new EntityNotFoundException("Искомый работник не найден");
            }

            _employeeStorage.Update(employee.Id, employee);
        }

        public void DeleteEmployee(Guid employeeId) 
        {
            var employee = _employeeStorage.GetById(employeeId);

            if (employee == null)
            {
                throw new EntityNotFoundException("Искомый работник не найден");
            }

            _employeeStorage.Delete(employeeId);
        }
    }
}
