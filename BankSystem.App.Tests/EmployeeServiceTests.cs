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
            _context = new BankSystemDbContext();
            _testDataGenerator = new TestDataGenerator();
            _employeeService = new EmployeeService(new EmployeeStorage(_context));
        }

        [Fact]
        public async Task GetEmployeeByIdShouldReturnEmployeeById() 
        {
            var employee = _testDataGenerator.GenerateEmployee();
            await _employeeService.AddEmployeeAsync(employee);

            var desiredEmployee = await _employeeService.GetEmployeeByIdAsync(employee.Id);

            Assert.NotNull(desiredEmployee);
            Assert.Equal(employee.PassportData, desiredEmployee.PassportData);

            await _employeeService.DeleteEmployeeAsync(employee.Id);
        }

        [Fact]
        public async Task AddEmployeeShouldAddEmployee() 
        {
            var employee = _testDataGenerator.GenerateEmployee();
            await _employeeService.AddEmployeeAsync(employee);

            var existingEmployee = await _employeeService.GetEmployeeByIdAsync(employee.Id);

            Assert.Equal(employee.PassportData, existingEmployee.PassportData);

            await _employeeService.DeleteEmployeeAsync(employee.Id);
        }

        [Fact]
        public async Task GetEmployeesByFilterShouldReturnFilteredEmployees()
        {
            var firstEmployee = _testDataGenerator.GenerateEmployee();
            var secondEmployee = _testDataGenerator.GenerateEmployee();

            await _employeeService.AddEmployeeAsync(firstEmployee);
            await _employeeService.AddEmployeeAsync(secondEmployee);

            var filteredEmployees = await _employeeService.FilterEmployeesAsync(e => e.PassportData == secondEmployee.PassportData);

            Assert.Single(filteredEmployees);

            await _employeeService.DeleteEmployeeAsync(firstEmployee.Id);
            await _employeeService.DeleteEmployeeAsync(secondEmployee.Id);
        }

        [Fact]
        public async Task UpdateEmployeeShouldUpdateExistingEmployee()
        {
            var existingEmployee = _testDataGenerator.GenerateEmployee();

            await _employeeService.AddEmployeeAsync(existingEmployee);

            existingEmployee.Contract = "Half-day";
            await _employeeService.UpdateEmployeeAsync(existingEmployee);

            var updatedEmployee = await _employeeService.GetEmployeeByIdAsync(existingEmployee.Id);

            Assert.Equal("Half-day", updatedEmployee.Contract);

            await _employeeService.DeleteEmployeeAsync(existingEmployee.Id);
        }
    }
}
