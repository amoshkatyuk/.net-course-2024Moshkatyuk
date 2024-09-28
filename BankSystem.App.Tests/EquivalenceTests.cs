using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests
{
    public class EquivalenceTests
    {
        TestDataGenerator testDataGenerator = new TestDataGenerator();

        [Fact]
        public void GetHashCodeNecessityPositivTest() // �� ��������������� Equals() � GetHashCode() � ������� Account � Client ������ ����� ������� �����������
        {
            var clientAccountDictionary = testDataGenerator.GenerateClientAccountDictionary();

            var existingClient = clientAccountDictionary.Keys.First();

            var testClient = new Client
            {
                Name = existingClient.Name,
                Surname = existingClient.Surname,
                PassportData = existingClient.PassportData,
                Age = existingClient.Age,
                TelephoneNumber = existingClient.TelephoneNumber
            };

            if (clientAccountDictionary.TryGetValue(testClient, out var account))
            {
                Assert.NotNull(account);
            }
            else 
            {
                Assert.Fail("� ������� �� ������ �������");
            }
        }

        [Fact]
        public void GetClientWithManyAccountsPositivTest() 
        {
            var clientAccountsDictionary = testDataGenerator.GenerateClientWithManyAccountsDictionary();

            var existingClient = clientAccountsDictionary.Keys.First();

            var testClient = new Client
            {
                Name= existingClient.Name,
                Surname= existingClient.Surname,
                PassportData= existingClient.PassportData,
                Age = existingClient.Age,
                TelephoneNumber = existingClient.TelephoneNumber
            };

            if (clientAccountsDictionary.TryGetValue(testClient, out var accounts))
            {
                Assert.NotNull(accounts);
                Assert.True(accounts.Count > 1);
            }
            else
            {
                Assert.Fail("� ������� �� ������� ��������");
            }
        }

        [Fact]
        public void GetEmployeePositivTest() // �� ��������������� ������� Equals() � GetHashCode() ���� ���������� � �������
        {
            var employeeList = testDataGenerator.GenerateEmployees(200);

            var existingEmployee = employeeList.First();

            var testEmployee = new Employee
            {
                Name = existingEmployee.Name,
                Surname = existingEmployee.Surname,
                PassportData = existingEmployee.PassportData,
                Age = existingEmployee.Age,
                Salary = existingEmployee.Salary,
                Position = existingEmployee.Position,
                Contract = existingEmployee.Contract
            };

            Assert.True(employeeList.Contains(testEmployee), "������ �������� ����������� � ������");
        }
    }
}