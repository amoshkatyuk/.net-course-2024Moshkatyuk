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
