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
    }
}
