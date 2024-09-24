using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PassportData { get; set; }
        public int Age { get; set; }

        public Person(string name, string surname, string passportData, int age) 
        {
            Name = name;
            Surname = surname;
            PassportData = passportData;
            Age = age;
        }
    }
}
