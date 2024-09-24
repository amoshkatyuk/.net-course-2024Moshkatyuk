using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Client : Person
    {
        public string BankAccount { get; set; }
        public string TelephoneNumber { get; set; }

        public Client(string name, string surname, string passportData, int age, string bankAccount, string telephoneNumber)
            : base(name, surname, passportData, age)
        {
            BankAccount = bankAccount;
            TelephoneNumber = telephoneNumber;
        }
    }
}
