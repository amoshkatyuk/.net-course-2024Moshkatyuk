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

        public async Task<Employee> GetByIdAsync(Guid employeeId)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Employee>> GetAsync(Expression<Func<Employee, bool>> filter)
        {
            return await _context.Employees
                .Where(filter)
                .ToListAsync();
        }

        public async Task UpdateAsync(Guid employeeId, Employee employee)
        {
            var existingEmployee = await GetByIdAsync(employeeId);
            _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid employeeId)
        {
            var employee = await GetByIdAsync(employeeId);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
