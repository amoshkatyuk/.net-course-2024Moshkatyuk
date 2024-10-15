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
    }
}
