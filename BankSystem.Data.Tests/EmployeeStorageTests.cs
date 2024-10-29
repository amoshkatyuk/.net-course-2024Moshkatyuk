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

        public EmployeeStorageTests()
        {

            _context = new BankSystemDbContext();
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
            await _employeeStorage.AddAsync(employee);

            var result = await _employeeStorage.GetByIdAsync(employee.Id);

            Assert.Equal(employee, result);

            await _employeeStorage.DeleteAsync(employee.Id);
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
            await _employeeStorage.AddAsync(employee);

            var result = await _employeeStorage.GetByIdAsync(employee.Id);

            Assert.Equal("Alex", result.Name);

            await _employeeStorage.DeleteAsync(employee.Id);
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
            await _employeeStorage.AddAsync(firstEmployee);

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
            await _employeeStorage.AddAsync(secondEmployee);

            var filteredEmployees = await _employeeStorage.GetAsync(e => e.Name == "Nick");

            Assert.Equal(filteredEmployees.First().Name, secondEmployee.Name);

            await _employeeStorage.DeleteAsync(firstEmployee.Id);

            await _employeeStorage.DeleteAsync(secondEmployee.Id);
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
            await _employeeStorage.AddAsync(existingEmployee);

            existingEmployee.Surname = "Stepanov";
            await _employeeStorage.UpdateAsync(existingEmployee.Id, existingEmployee);

            var updatedEmployee = await _employeeStorage.GetByIdAsync(existingEmployee.Id);

            Assert.Equal("Stepanov", updatedEmployee.Surname);

            await _employeeStorage.DeleteAsync(existingEmployee.Id);
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
            await _employeeStorage.AddAsync(employee);

            await _employeeStorage.DeleteAsync(employee.Id);

            var result = await _employeeStorage.GetByIdAsync(employee.Id);

            Assert.Null(result);
        }
    }
}
