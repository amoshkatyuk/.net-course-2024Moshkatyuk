using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Exceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(string message) : base(message) { }
    }
}
