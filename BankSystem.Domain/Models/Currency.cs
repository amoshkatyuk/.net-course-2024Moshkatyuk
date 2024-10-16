using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Currency
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public ICollection<Account> Accounts { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Currency currency)
            {
                return Id == currency.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
