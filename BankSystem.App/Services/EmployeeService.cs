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

        public async Task<Employee> GetEmployeeByIdAsync(Guid employeeId)
        {
            var employee = await _employeeStorage.GetByIdAsync(employeeId);

            if (employee == null)
            {
                throw new EntityNotFoundException("Искомый работник не найден");
            }

            return employee;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.PassportData))
            {
                throw new NoPassportDataException("Работник не имеет паспортных данных");
            }

            if (employee.Age < 18)
            {
                throw new UnderagePeopleException("Несовершеннолетний работник");
            }

            await _employeeStorage.AddAsync(employee);
        }

        public async Task<List<Employee>> FilterEmployeesAsync(Func<Employee, bool> filter)
        {
            return await _employeeStorage.GetAsync(filter);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _employeeStorage.GetByIdAsync(employee.Id);

            if (existingEmployee == null)
            {
                throw new EntityNotFoundException("Искомый работник не найден");
            }

            await _employeeStorage.UpdateAsync(employee.Id, employee);
        }

        public async Task DeleteEmployeeAsync(Guid employeeId) 
        {
            var employee = await _employeeStorage.GetByIdAsync(employeeId);

            if (employee == null)
            {
                throw new EntityNotFoundException("Искомый работник не найден");
            }

            await _employeeStorage.DeleteAsync(employeeId);
        }
    }
}
