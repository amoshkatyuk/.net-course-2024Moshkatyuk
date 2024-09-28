using BankSystem.Domain;
using BankSystem.Domain.Models;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Person firstPerson = new Person { Name = "Ivan", Surname = "Ivanov", Age = 18, PassportData = "AB1234567890" };

            Person secondPerson = new Person { Name = "Ivan", Surname = "Ivanov", Age = 18, PassportData = "AB1234567890" };

            Assert.Equal(firstPerson, secondPerson);
        }
    }
}