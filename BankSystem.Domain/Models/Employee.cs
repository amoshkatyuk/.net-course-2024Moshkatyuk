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
        public string Position { get; set; } // оставил намеренно, чтобы пока не менять структуру моделей
        public string Contract { get; set; }
        
        public override bool Equals(object obj)
        {
            if (obj is Employee employee)
            {
                return Id == employee.Id;
            }
            return false;
        }

        public override int GetHashCode() 
        {
            return Id.GetHashCode();
        }
    }
}
