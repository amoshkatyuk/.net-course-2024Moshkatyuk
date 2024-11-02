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
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public EmployeeServiceTests()
        {
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=BankSystemDb;Username=postgres;Password=admin")
                .Options;

            _context = new BankSystemDbContext(options);
            _testDataGenerator = new TestDataGenerator();
            _employeeService = new EmployeeService(new EmployeeStorage(_context));
        }

        [Fact]
        public async Task GetEmployeeByIdShouldReturnEmployeeById() 
        {
            var employee = _testDataGenerator.GenerateEmployee();
            await _employeeService.AddEmployeeAsync(employee, _cancellationToken);

            var desiredEmployee = await _employeeService.GetEmployeeByIdAsync(employee.Id, _cancellationToken);

            Assert.NotNull(desiredEmployee);
            Assert.Equal(employee.PassportData, desiredEmployee.PassportData);

            await _employeeService.DeleteEmployeeAsync(employee.Id, _cancellationToken);
        }

        [Fact]
        public async Task AddEmployeeShouldAddEmployee() 
        {
            var employee = _testDataGenerator.GenerateEmployee();
            await _employeeService.AddEmployeeAsync(employee, _cancellationToken);

            var existingEmployee = await _employeeService.GetEmployeeByIdAsync(employee.Id, _cancellationToken);

            Assert.Equal(employee.PassportData, existingEmployee.PassportData);

            await _employeeService.DeleteEmployeeAsync(employee.Id, _cancellationToken);
        }

        [Fact]
        public async Task GetEmployeesByFilterShouldReturnFilteredEmployees()
        {
            var firstEmployee = _testDataGenerator.GenerateEmployee();
            var secondEmployee = _testDataGenerator.GenerateEmployee();

            await _employeeService.AddEmployeeAsync(firstEmployee, _cancellationToken);
            await _employeeService.AddEmployeeAsync(secondEmployee, _cancellationToken);

            var filteredEmployees = await _employeeService.FilterEmployeesAsync(e => e.PassportData == secondEmployee.PassportData, _cancellationToken);

            Assert.Single(filteredEmployees);

            await _employeeService.DeleteEmployeeAsync(firstEmployee.Id, _cancellationToken);
            await _employeeService.DeleteEmployeeAsync(secondEmployee.Id, _cancellationToken);
        }

        [Fact]
        public async Task UpdateEmployeeShouldUpdateExistingEmployee()
        {
            var existingEmployee = _testDataGenerator.GenerateEmployee();

            await _employeeService.AddEmployeeAsync(existingEmployee, _cancellationToken);

            existingEmployee.Contract = "Half-day";
            await _employeeService.UpdateEmployeeAsync(existingEmployee, _cancellationToken);

            var updatedEmployee = await _employeeService.GetEmployeeByIdAsync(existingEmployee.Id, _cancellationToken);

            Assert.Equal("Half-day", updatedEmployee.Contract);

            await _employeeService.DeleteEmployeeAsync(existingEmployee.Id, _cancellationToken);
        }
    }
}
