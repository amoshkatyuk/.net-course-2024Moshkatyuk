using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class ClientStorage : IClientStorage
    {
        private Dictionary<Client, List<Account>> _clients;

        public ClientStorage()
        {
            _clients = new Dictionary<Client, List<Account>>();
        }

        public void Add(Client client)
        {
            _clients.Add(client, new List<Account>());
        }

        public List<Client> Get(Func<Client, bool> filter)
        {
            return _clients.Keys.Where(filter).ToList();
        }

        public void Update(Client client) 
        {
            if (!_clients.ContainsKey(client))
            {
                throw new EntityNotFoundException("Клиент не найден");
            }

            var existingAccounts = _clients[client];

            _clients.Remove(client);
            _clients.Add(client, existingAccounts);
        }

        public void Delete(Client client) 
        {
            if (!_clients.ContainsKey(client))
            {
                throw new EntityNotFoundException("Клиент не найден");
            }

            _clients.Remove(client);
        }

        public void AddAccount(Client client, Account account)
        {
            if (_clients.ContainsKey(client))
            {
                _clients[client].Add(account);
            }
        }

        public void UpdateAccount(Client client, Account updatedAccount)
        {
            var accounts = _clients[client];
            var existingAccount = accounts.FirstOrDefault(a => a.Currency == updatedAccount.Currency);

            existingAccount.Amount = updatedAccount.Amount;
        }

        public void DeleteAccount(Client client, Account account) 
        {
            _clients[client].Remove(account);
        }

        public bool ClientExists(Client client)
        {
            return _clients.Keys.Any(existingClient => existingClient.Equals(client));
        }

        public List<Client> GetClients() 
        {
            return _clients.Keys.ToList();
        }

        public List<Account> GetAccounts(Client client)
        {
            if (_clients.ContainsKey(client))
            {
                return _clients[client];
            }

            return new List<Account>();
        }

        public Client GetYoungestClient() 
        {
            return _clients.Keys.OrderBy(c => c.Age).FirstOrDefault();
        }

        public Client GetOldestClient() 
        {
            return _clients.Keys.OrderByDescending(c => c.Age).FirstOrDefault();
        }

        public double GetClientsAverageAge() 
        {
            return _clients.Keys.Average(c => c.Age);
        }
    }
}
