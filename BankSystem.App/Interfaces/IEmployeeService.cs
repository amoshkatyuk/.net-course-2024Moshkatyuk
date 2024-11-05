using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IEmployeeService
    {
        public Task<Employee> GetEmployeeByIdAsync(Guid employeeId, CancellationToken cancellationToken);
        public Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        public Task UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken);
        public Task DeleteEmployeeAsync(Guid employeeId, CancellationToken cancellationToken);
        public Task<List<Employee>> FilterEmployeesAsync(Expression<Func<Employee, bool>> filter, CancellationToken cancellationToken);
    }
}
