using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Employee : Person
    {
        public decimal Salary { get; set; }
        public string Position { get; set; }
        public string Contract { get; set; }

        public Employee(string name, string surname, string passportData, int age, decimal salary, string position)
            : base(name, surname, passportData, age)
        {
            Salary = salary;
            Position = position;
        }
    }
}
