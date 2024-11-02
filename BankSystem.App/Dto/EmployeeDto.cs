using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Dto
{
    public class EmployeeDto
    {
        public string FullName { get; set; }
        public string PassportData { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public decimal Salary { get; set; }
        public string Position { get; set; }
        public string Contract { get; set; }
    }
}
