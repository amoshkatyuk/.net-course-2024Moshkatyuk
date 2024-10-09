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
    public class EmployeeStorage : IEmployeeStorage
    {
        private List<Employee> _employees;

        public EmployeeStorage() 
        {
            _employees = new List<Employee>();
        }

        public void Add(Employee employee) 
        {
            _employees.Add(employee);
        }

        public List<Employee> Get(Func<Employee, bool> filter)
        {
            return _employees.Where(filter).ToList();
        }

        public void Update(Employee employee) 
        {
            if (!_employees.Contains(employee))
            {
                throw new EntityNotFoundException("Работник не найден");
            }

            _employees.Remove(employee);

            _employees.Add(employee);
        }

        public void Delete(Employee employee) 
        {
            if (!_employees.Contains(employee))
            {
                throw new Exception("Работник не найден");
            }
            
            _employees.Remove(employee);
        }

        public Employee GetYoungestEmployee()
        {
            return _employees.OrderBy(e => e.Age).FirstOrDefault();
        }

        public bool EmployeeExists(Employee employee)
        {
            return _employees.Any(e => e.Equals(employee));
        }

        public Employee GetOldestEmployee()
        {
            return _employees.OrderByDescending(e => e.Age).FirstOrDefault();
        }

        public double GetEmployeesAverageAge()
        {
            return _employees.Average(e => e.Age);
        }
    }
}
