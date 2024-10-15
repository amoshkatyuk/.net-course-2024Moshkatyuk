using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IClientStorage : IStorage<Client>
    {
        public void AddAccount(Guid clientId, Account account);
        public void DeleteAccount(Guid clientId, Guid accountId);
    }
}
