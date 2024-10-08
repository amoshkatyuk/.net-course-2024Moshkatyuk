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
        public void AddAccount(Client client, List<Account> account);
        public void UpdateAccount(Client client, Account updatedAccount);
        public void DeleteAccount(Client client, Account account);

        public bool ClientExists(Client client);
        public List<Account> GetAccounts(Client client);
    }
}
