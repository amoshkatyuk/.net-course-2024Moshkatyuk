using BankSystem.App.Exceptions;
using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Tests
{
    public class EmployeeServiceTests
    {
        private readonly BankSystemDbContext _context;
        private readonly EmployeeService _employeeService;
        private readonly TestDataGenerator _testDataGenerator;

        public EmployeeServiceTests()
        {
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=BankSystemDb;Username=postgres;Password=admin")
                .Options;

            _context = new BankSystemDbContext();
            _testDataGenerator = new TestDataGenerator();
            _employeeService = new EmployeeService(new EmployeeStorage(_context));
        }

        [Fact]
        public void GetEmployeeByIdShouldReturnEmployeeById() 
        {
            var employee = _testDataGenerator.GenerateEmployee();
            _employeeService.AddEmployee(employee);

            var desiredEmployee = _employeeService.GetEmployeeById(employee.Id);

            Assert.NotNull(desiredEmployee);
            Assert.Equal(employee.PassportData, desiredEmployee.PassportData);

            _employeeService.DeleteEmployee(employee.Id);
        }

        [Fact]
        public void AddEmployeeShouldAddEmployee() 
        {
            var employee = _testDataGenerator.GenerateEmployee();
            _employeeService.AddEmployee(employee);

            var existingEmployee = _employeeService.GetEmployeeById(employee.Id);

            Assert.Equal(employee.PassportData, existingEmployee.PassportData);

            _employeeService.DeleteEmployee(employee.Id);
        }

        [Fact]
        public void GetEmployeesByFilterShouldReturnFilteredEmployees()
        {
            var firstEmployee = _testDataGenerator.GenerateEmployee();
            var secondEmployee = _testDataGenerator.GenerateEmployee();

            _employeeService.AddEmployee(firstEmployee);
            _employeeService.AddEmployee(secondEmployee);

            var filteredEmployees = _employeeService.FilterEmployees(e => e.PassportData == secondEmployee.PassportData);

            Assert.Single(filteredEmployees);

            _employeeService.DeleteEmployee(firstEmployee.Id);
            _employeeService.DeleteEmployee(secondEmployee.Id);
        }

        [Fact]
        public void UpdateEmployeeShouldUpdateExistingEmployee()
        {
            var existingEmployee = _testDataGenerator.GenerateEmployee();

            _employeeService.AddEmployee(existingEmployee);

            existingEmployee.Contract = "Half-day";
            _context.Employees.Update(existingEmployee);

            var updatedEmployee = _employeeService.GetEmployeeById(existingEmployee.Id);

            Assert.Equal("Half-day", updatedEmployee.Contract);

            _employeeService.DeleteEmployee(existingEmployee.Id);
        }
    }
}
