using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage
    {
        private List<Employee> _employees;

        public EmployeeStorage() 
        {
            _employees = new List<Employee>();
        }

        public void AddEmployees(List<Employee> employees) 
        {
            _employees.AddRange(employees);
        }

        public void AddEmployee(Employee employee) 
        {
            _employees.Add(employee);
        }

        public List<Employee> FilterEmployees(string name, string surname, string passportData, decimal? salary, string position, string contract, DateTime? birthDateFrom, DateTime? birthDateTo)
        {
            return _employees.Where(e =>
            (string.IsNullOrEmpty(name) || e.Name.Contains(name)) &&
            (string.IsNullOrEmpty(surname) || e.Surname.Contains(surname)) &&
            (string.IsNullOrEmpty(passportData) || e.PassportData.Contains(passportData)) &&
            (!salary.HasValue || e.Salary == salary.Value) &&
            (string.IsNullOrEmpty(position) || e.Position.Contains(position)) &&
            (string.IsNullOrEmpty(contract) || e.Contract.Contains(contract)) &&
            (!birthDateFrom.HasValue || e.BirthDate >= birthDateFrom.Value) &&
            (!birthDateTo.HasValue || e.BirthDate <= birthDateTo.Value))
            .ToList();
        }

        public Employee GetYoungestEmployee()
        {
            return _employees.OrderBy(e => e.Age).FirstOrDefault();
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
