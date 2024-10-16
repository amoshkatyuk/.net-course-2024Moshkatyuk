using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }

        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Account account)
            {
                return Id == account.Id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
