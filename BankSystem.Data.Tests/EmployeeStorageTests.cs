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
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase(databaseName: "TestBankSystemDb")
                .Options;

            _context = new BankSystemDbContext();
            _employeeStorage = new EmployeeStorage(_context);
        }

        [Fact]
        public void GetByIdShouldReturnEmployeeById() 
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
            _employeeStorage.Add(employee);

            var result = _employeeStorage.GetById(employee.Id);

            Assert.Equal(employee, result);

            _employeeStorage.Delete(employee.Id);
        }

        [Fact]
        public void AddEmployeeShouldAddEmployee() 
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
            _employeeStorage.Add(employee);

            var result = _employeeStorage.GetById(employee.Id);

            Assert.Equal("Alex", result.Name);

            _employeeStorage.Delete(employee.Id);
        }

        [Fact]
        public void GetEmployeesByFilterShouldReturnFilteredEmployees() 
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
            _employeeStorage.Add(firstEmployee);

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
            _employeeStorage.Add(secondEmployee);

            var filteredEmployees = _employeeStorage.Get(e => e.Name == "Nick");

            Assert.Equal(filteredEmployees.First().Name, secondEmployee.Name);

            _employeeStorage.Delete(firstEmployee.Id);

            _employeeStorage.Delete(secondEmployee.Id);
        }

        [Fact]
        public void UpdateEmployeeShouldUpdateExistingEmployee() 
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
            _employeeStorage.Add(existingEmployee);

            existingEmployee.Surname = "Stepanov";
            _employeeStorage.Update(existingEmployee);

            var updatedEmployee = _employeeStorage.GetById(existingEmployee.Id);

            Assert.Equal("Stepanov", updatedEmployee.Surname);

            _employeeStorage.Delete(existingEmployee.Id);
        }

        [Fact]
        public void DeleteEmployeeShouldDeleteExistingEmployee() 
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
            _employeeStorage.Add(employee);

            _employeeStorage.Delete(employee.Id);

            var result = _employeeStorage.GetById(employee.Id);

            Assert.Null(result);
        }
    }
}
