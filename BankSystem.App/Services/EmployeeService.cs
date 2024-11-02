using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using System.Linq.Expressions;
using System.Threading;

namespace BankSystem.App.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IStorage<Employee> _employeeStorage;

        public EmployeeService(IStorage<Employee> employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid employeeId, CancellationToken cancellationToken)
        {
            var employee = await _employeeStorage.GetByIdAsync(employeeId, cancellationToken);

            if (employee == null)
            {
                throw new EntityNotFoundException("Искомый работник не найден");
            }

            return employee;
        }

        public async Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(employee.PassportData))
            {
                throw new NoPassportDataException("Работник не имеет паспортных данных");
            }

            if (employee.Age < 18)
            {
                throw new UnderagePeopleException("Несовершеннолетний работник");
            }

            await _employeeStorage.AddAsync(employee, cancellationToken);
        }

        public async Task<List<Employee>> FilterEmployeesAsync(Expression<Func<Employee, bool>> filter, CancellationToken cancellationToken)
        {
            return await _employeeStorage.GetAsync(filter, cancellationToken);
        }

        public async Task UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken)
        {
            var existingEmployee = await _employeeStorage.GetByIdAsync(employee.Id, cancellationToken);

            if (existingEmployee == null)
            {
                throw new EntityNotFoundException("Искомый работник не найден");
            }

            await _employeeStorage.UpdateAsync(employee.Id, employee, cancellationToken);
        }

        public async Task DeleteEmployeeAsync(Guid employeeId, CancellationToken cancellationToken) 
        {
            var employee = await _employeeStorage.GetByIdAsync(employeeId, cancellationToken);

            if (employee == null)
            {
                throw new EntityNotFoundException("Искомый работник не найден");
            }

            await _employeeStorage.DeleteAsync(employeeId, cancellationToken);
        }
    }
}
