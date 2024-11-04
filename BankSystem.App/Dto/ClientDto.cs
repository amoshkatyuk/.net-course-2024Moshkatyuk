using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Dto
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PassportData { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string TelephoneNumber { get; set; }
    }
}
