using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage : IStorage<Employee>
    {
        private readonly BankSystemDbContext _context;

        public EmployeeStorage(BankSystemDbContext context) 
        {
            _context = context;
        }

        public async Task<Employee> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);
        }

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken)
        {
            await _context.Employees.AddAsync(employee, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Employee>> GetAsync(Expression<Func<Employee, bool>> filter, CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Guid employeeId, Employee employee, CancellationToken cancellationToken)
        {
            var existingEmployee = await GetByIdAsync(employeeId, cancellationToken);
            _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid employeeId, CancellationToken cancellationToken)
        {
            var employee = await GetByIdAsync(employeeId, cancellationToken);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
