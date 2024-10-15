using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Employee GetById(Guid employeeId)
        {
            return _context.Employees.FirstOrDefault(e => e.Id == employeeId);
        }

        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public List<Employee> Get(Func<Employee, bool> filter)
        {
            return _context.Employees
                .Where(filter)
                .ToList();
        }

        public void Update(Employee employee)
        {
            var existingEmployee = GetById(employee.Id);
            _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
            _context.SaveChanges();
        }

        public void Delete(Guid employeeId)
        {
            var employee = GetById(employeeId);
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}
