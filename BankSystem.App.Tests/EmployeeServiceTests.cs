using BankSystem.App.Exceptions;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Tests
{
    public class EmployeeServiceTests
    {
        private EmployeeService _employeeService;
        private EmployeeStorage _employeeStorage;
        private TestDataGenerator _testDataGenerator;

        public EmployeeServiceTests()
        {
            _employeeStorage = new EmployeeStorage();
            _employeeService = new EmployeeService(_employeeStorage);
            _testDataGenerator = new TestDataGenerator();
        }

        [Fact]
        public void AddEmployeeShouldAddEmployee()
        {
            var newEmployee = new Employee
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
                Position = "Manager",
                Salary = 50000,
                Contract = "Full-time"
            };

            _employeeService.AddEmployee(newEmployee);
            var employees = _employeeService.FilterEmployees(passportData: newEmployee.PassportData);

            Assert.NotNull(employees);
        }

        [Fact]
        public void AddEmployeeShouldThrowNoPassportDataException()
        {
            var noPassportDataEmployee = new Employee
            {
                Name = "Alex",
                Surname = "Ivanov",
                BirthDate = DateTime.Today.AddYears(-25),
            };

            var exception = Assert.Throws<NoPassportDataException>(() => _employeeService.AddEmployee(noPassportDataEmployee));
            Assert.Equal("Работник не имеет паспортных данных", exception.Message);
        }

        [Fact]
        public void AddEmployeeShouldThrowUnderageException()
        {
            var underageEmployee = new Employee
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-15),
            };

            var exception = Assert.Throws<UnderagePeopleException>(() => _employeeService.AddEmployee(underageEmployee));
            Assert.Equal("Несовершеннолетний работник", exception.Message);
        }

        [Fact]
        public void FilterEmployeesShoultReturnFilteredEmployees()
        {
            var employees = _testDataGenerator.GenerateEmployees(3);
            employees[0].Name = "Alex";
            employees[0].Surname = "Moshkatyuk";
            employees[0].PassportData = "AA1234567";
            employees[0].BirthDate = DateTime.Today.AddYears(-25);
            _employeeService.AddEmployee(employees[0]);

            employees[1].Name = "Michael";
            employees[1].Surname = "Jordan";
            employees[1].PassportData = "BB7654321";
            employees[1].BirthDate = DateTime.Today.AddYears(-25);
            _employeeService.AddEmployee(employees[1]);

            employees[2].Name = "Alex";
            employees[2].Surname = "Ivanov";
            employees[2].PassportData = "CC1234377";
            employees[2].BirthDate = DateTime.Today.AddYears(-35);
            _employeeService.AddEmployee(employees[2]);

            var filteredEmployees = _employeeService.FilterEmployees("Alex", null, null, null, null);

            Assert.Equal(2, filteredEmployees.Count);
            Assert.All(filteredEmployees, c => Assert.Equal("Alex", c.Name));
        }

        [Fact]
        public void UpdateEmployeeShouldReturnUpdatedEmployee() 
        {
            var employee = new Employee
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
            };
            _employeeService.AddEmployee(employee);

            var updatedEmployee = new Employee
            {
                Name = "Ivan",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
            };

            _employeeService.UpdateEmployee(updatedEmployee);

            var employees = _employeeService.FilterEmployees(passportData: employee.PassportData);

            Assert.True(employees[0].Name == "Ivan");
        }

        [Fact]
        public void DeleteEmployeeShouldDeletedEmployee() 
        {
            var employee = new Employee
            {
                Name = "Alex",
                Surname = "Ivanov",
                PassportData = "AB123456789",
                BirthDate = DateTime.Today.AddYears(-25),
            };
            _employeeService.AddEmployee(employee);

            _employeeService.DeleteEmployee(employee);

            var employees = _employeeService.FilterEmployees(passportData: employee.PassportData);

            Assert.True(employees.Count == 0);
        }
    }
}
