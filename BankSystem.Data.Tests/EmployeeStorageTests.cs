using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
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

        public EmployeeStorageTests()
        {
            _employeeStorage = new EmployeeStorage();
        }

        [Fact]
        public void AddEmployeeShouldAddEmployee()
        {
            var employee = new Employee
            {
                Name = "Igor",
                Surname = "Petrov",
                PassportData = "AB987654321",
                BirthDate = DateTime.Parse("1984-10-01"),
                Salary = 6000.00m,
                Position = "Financial Director",
                Contract = "Контракт не составлен."
            };
            _employeeStorage.Add(employee);

            var result = _employeeStorage.Get(e => e.PassportData == "AB987654321");

            Assert.True(result.Any());
            Assert.Equal("AB987654321", result.First().PassportData);
        }

        [Fact]
        public void GetYoungestEmployeeShouldReturnYoungestEmployee()
        {
            var firstEmployee = new Employee
            {
                Name = "Igor",
                Surname = "Petrov",
                PassportData = "AB987654321",
                BirthDate = DateTime.Today.AddYears(-25),
                Salary = 6000.00m,
                Position = "Financial Director",
                Contract = "Контракт не составлен."
            };
            _employeeStorage.Add(firstEmployee);

            var secondEmployee = new Employee
            {
                Name = "Igor",
                Surname = "Petrov",
                PassportData = "AB987654321",
                BirthDate = DateTime.Today.AddYears(-32),
                Salary = 6000.00m,
                Position = "Financial Director",
                Contract = "Контракт не составлен."
            };
            _employeeStorage.Add(secondEmployee);

            var youngestEmployee = _employeeStorage.GetYoungestEmployee();

            Assert.Equal(firstEmployee.Name, youngestEmployee.Name);
        }

        [Fact]
        public void GetOldestEmployeeShouldReturnOldestEmployee()
        {
            var firstEmployee = new Employee
            {
                Name = "Igor",
                Surname = "Petrov",
                PassportData = "AB987654321",
                BirthDate = DateTime.Today.AddYears(-25),
                Salary = 6000.00m,
                Position = "Financial Director",
                Contract = "Контракт не составлен."
            };
            _employeeStorage.Add(firstEmployee);

            var secondEmployee = new Employee
            {
                Name = "Igor",
                Surname = "Petrov",
                PassportData = "AB987654321",
                BirthDate = DateTime.Today.AddYears(-32),
                Salary = 6000.00m,
                Position = "Financial Director",
                Contract = "Контракт не составлен."
            };
            _employeeStorage.Add(secondEmployee);

            var oldestEmployee = _employeeStorage.GetYoungestEmployee();

            Assert.Equal(secondEmployee.Name, oldestEmployee.Name);
        }

        [Fact]
        public void GetEmployeesAverageAgeReturnAverageAge()
        {
            var firstEmployee = new Employee
            {
                Name = "Igor",
                Surname = "Petrov",
                PassportData = "AB987654321",
                BirthDate = DateTime.Today.AddYears(-20),
                Salary = 6000.00m,
                Position = "Financial Director",
                Contract = "Контракт не составлен."
            };
            _employeeStorage.Add(firstEmployee);

            var secondEmployee = new Employee
            {
                Name = "Igor",
                Surname = "Petrov",
                PassportData = "AB987654321",
                BirthDate = DateTime.Today.AddYears(-30),
                Salary = 6000.00m,
                Position = "Financial Director",
                Contract = "Контракт не составлен."
            };
            _employeeStorage.Add(secondEmployee);

            var employeesAverageAge = _employeeStorage.GetEmployeesAverageAge();

            Assert.Equal(25, employeesAverageAge);
        }
    }
}
