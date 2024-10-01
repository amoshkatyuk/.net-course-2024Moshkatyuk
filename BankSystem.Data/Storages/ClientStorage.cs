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
        private List<Client> _clients;

        public ClientStorage()
        {
            _clients = new List<Client>();
        }

        public void AddClients(List<Client> clients)
        {
            _clients.AddRange(new List<Client>());
        }

        public void AddClient(Client client) 
        {
            _clients.Add(client);
        }

        public Client GetYoungestClient() 
        {
            return _clients.OrderBy(c => c.Age).FirstOrDefault();
        }

        public Client GetOldestClient() 
        {
            return _clients.OrderBy(c => c.Age).LastOrDefault();
        }

        public double GetClientsAverageAge() 
        {
            return _clients.Average(c => c.Age);
        }
    }
}
