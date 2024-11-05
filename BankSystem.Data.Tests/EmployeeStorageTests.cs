using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Tests
{
    public class EmployeeStorageTests
    {
        private EmployeeStorage _employeeStorage;
        private BankSystemDbContext _context;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public EmployeeStorageTests()
        {

            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=BankSystemDb;Username=postgres;Password=admin")
                .Options;

            _context = new BankSystemDbContext(options);
            _employeeStorage = new EmployeeStorage(_context);
        }

        [Fact]
        public async Task GetByIdShouldReturnEmployeeById() 
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AA123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Position = "Manager",
                Salary = 50000,
                Contract = "Full-time"
            };
            await _employeeStorage.AddAsync(employee, _cancellationToken);

            var result = await _employeeStorage.GetByIdAsync(employee.Id, _cancellationToken);

            Assert.Equal(employee, result);

            await _employeeStorage.DeleteAsync(employee.Id, _cancellationToken);
        }

        [Fact]
        public async Task AddEmployeeShouldAddEmployee() 
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Position = "Manager",
                Salary = 50000,
                Contract = "Full-time"
            };
            await _employeeStorage.AddAsync(employee, _cancellationToken);

            var result = await _employeeStorage.GetByIdAsync(employee.Id, _cancellationToken);

            Assert.Equal("Alex", result.Name);

            await _employeeStorage.DeleteAsync(employee.Id, _cancellationToken);
        }

        [Fact]
        public async Task GetEmployeesByFilterShouldReturnFilteredEmployees() 
        {
            var firstEmployee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AC123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Position = "Manager",
                Salary = 50000,
                Contract = "Full-time"
            };
            await _employeeStorage.AddAsync(firstEmployee, _cancellationToken);

            var secondEmployee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Nick",
                Surname = "Ivanov",
                PassportData = "AD123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Position = "Manager",
                Salary = 50000,
                Contract = "Full-time"
            };
            await _employeeStorage.AddAsync(secondEmployee, _cancellationToken);

            var filteredEmployees = await _employeeStorage.GetAsync(e => e.Name == "Nick", _cancellationToken);

            Assert.Equal(filteredEmployees.First().Name, secondEmployee.Name);

            await _employeeStorage.DeleteAsync(firstEmployee.Id, _cancellationToken);

            await _employeeStorage.DeleteAsync(secondEmployee.Id, _cancellationToken);
        }

        [Fact]
        public async Task UpdateEmployeeShouldUpdateExistingEmployee() 
        {
            var existingEmployee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AE123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Position = "Manager",
                Salary = 50000,
                Contract = "Full-time"
            };
            await _employeeStorage.AddAsync(existingEmployee, _cancellationToken);

            existingEmployee.Surname = "Stepanov";
            await _employeeStorage.UpdateAsync(existingEmployee.Id, existingEmployee, _cancellationToken);

            var updatedEmployee = await _employeeStorage.GetByIdAsync(existingEmployee.Id, _cancellationToken);

            Assert.Equal("Stepanov", updatedEmployee.Surname);

            await _employeeStorage.DeleteAsync(existingEmployee.Id, _cancellationToken);
        }

        [Fact]
        public async Task DeleteEmployeeShouldDeleteExistingEmployee() 
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AE123456789",
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Position = "Manager",
                Salary = 50000,
                Contract = "Full-time"
            };
            await _employeeStorage.AddAsync(employee, _cancellationToken);

            await _employeeStorage.DeleteAsync(employee.Id, _cancellationToken);

            var result = await _employeeStorage.GetByIdAsync(employee.Id, _cancellationToken);

            Assert.Null(result);
        }
    }
}
