using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    [NotMapped]
    public class CurrencyApiResponse
    {
        public int Error { get; set; }
        public string ErrorMessage { get; set; }
        public decimal Amount { get; set; }
    }
}
