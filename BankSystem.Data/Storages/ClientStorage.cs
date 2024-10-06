using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class ClientStorage
    {
        private Dictionary<Client, List<Account>> _clients;

        public ClientStorage()
        {
            _clients = new Dictionary<Client, List<Account>>();
        }

        public void AddClients(Dictionary<Client, List<Account>> clients)
        {
            foreach (var client in clients)
            {
                _clients.Add(client.Key, client.Value);
            }
        }

        public bool ClientExists(Client client)
        {
            return _clients.Keys.Any(existingClient => existingClient.Equals(client));
        }

        public List<Client> GetClients() 
        {
            return _clients.Keys.ToList();
        }

        public void AddAccountToClient(Client client, Account account) 
        {
            if (_clients.ContainsKey(client)) 
            {
                _clients[client].Add(account);
            }
        }

        public void UpdateClientAccount(Client client, Account updatedAccount) 
        {
            var accounts = _clients[client];
            var existingAccount = accounts.FirstOrDefault(a => a.Currency == updatedAccount.Currency);

            existingAccount.Amount = updatedAccount.Amount;
        }

        public List<Client> FilterClients(string name, string surname, string passportData, string telephoneNumber, DateTime? birthDateFrom, DateTime? birthDateTo)
        {
            return _clients.Keys
                .Where(c =>
                (string.IsNullOrEmpty(name) || c.Name.Contains(name)) &&
                (string.IsNullOrEmpty(surname) || c.Surname.Contains(surname)) &&
                (string.IsNullOrEmpty(passportData) || c.PassportData.Contains(passportData)) &&
                (string.IsNullOrEmpty(telephoneNumber) || c.TelephoneNumber.Contains(telephoneNumber)) &&
                (!birthDateFrom.HasValue || c.Age >= birthDateFrom.Value.Year) &&
                (!birthDateTo.HasValue || c.Age <= birthDateTo.Value.Year))
                .ToList();
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
