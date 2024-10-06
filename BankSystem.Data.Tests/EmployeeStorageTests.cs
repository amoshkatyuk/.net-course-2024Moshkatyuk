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
        TestDataGenerator testDataGenerator = new TestDataGenerator();

        public EmployeeStorageTests()
        {
            _employeeStorage = new EmployeeStorage();
        }

        [Fact]
        public void AddEmployeesShouldAddEmployees()
        {
            var employees = testDataGenerator.GenerateEmployees(1000);
            _employeeStorage.AddEmployees(employees);

            Assert.True(employees.Count == 1000);
        }

        [Fact]
        public void GetYoungestEmployeeShouldReturnYoungestEmployee()
        {
            var employees = testDataGenerator.GenerateEmployees(1000);

            _employeeStorage.AddEmployees(employees);

            var existingEmployee = employees.First();

            var testEmployee = new Employee
            {
                Name = existingEmployee.Name,
                Surname = existingEmployee.Surname,
                PassportData = existingEmployee.PassportData,
                Age = 16,
                Salary = existingEmployee.Salary,
                Position = existingEmployee.Position,
                Contract = existingEmployee.Contract
            };

            _employeeStorage.AddEmployee(testEmployee);

            var youngestEmployee = _employeeStorage.GetYoungestEmployee();

            Assert.Equal(testEmployee.Name, youngestEmployee.Name);
        }

        [Fact]
        public void GetOldestEmployeeShouldReturnOldestEmployee()
        {
            var employees = testDataGenerator.GenerateEmployees(1000);

            _employeeStorage.AddEmployees(employees);

            var existingEmployee = employees.First();

            var testEmployee = new Employee
            {
                Name = existingEmployee.Name,
                Surname = existingEmployee.Surname,
                PassportData = existingEmployee.PassportData,
                Age = 100,
                Salary = existingEmployee.Salary,
                Position = existingEmployee.Position,
                Contract = existingEmployee.Contract
            };

            _employeeStorage.AddEmployee(testEmployee);

            var oldestEmployee = _employeeStorage.GetOldestEmployee();

            Assert.Equal(testEmployee.Name, oldestEmployee.Name);
        }

        [Fact]
        public void GetEmployeesAverageAgeReturnAverageAge()
        {
            var firstTestEmployee = new Employee
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Parse("1964-10-01"),
                Salary = 8000.00m,
                Position = "Director",
                Contract = "Контракт не составлен."
            };

            var secondTestEmployee = new Employee
            {
                Name = "Igor",
                Surname = "Petrov",
                PassportData = "AB987654321",
                BirthDate = DateTime.Parse("1984-10-01"),
                Salary = 6000.00m,
                Position = "Financial Director",
                Contract = "Контракт не составлен."
            };

            _employeeStorage.AddEmployee(firstTestEmployee);
            _employeeStorage.AddEmployee(secondTestEmployee);

            var employeesAverageAge = _employeeStorage.GetEmployeesAverageAge();

            Assert.Equal(50, employeesAverageAge);
        }
    }
}
