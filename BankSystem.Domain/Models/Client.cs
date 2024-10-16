using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Client : Person
    {
        public string TelephoneNumber { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Client client)
            {
                return Id == client.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
