using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public struct Currency
    {
        public string Type { get; set; }
        public double Value { get; set; }

        public Currency(string type, double value)
        {
            Type = type;
            Value = value;
        }
    }
}
