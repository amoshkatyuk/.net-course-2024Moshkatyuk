using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Exceptions;
using BankSystem.Domain.Models;
using BankSystem.App.Interfaces;

namespace BankSystem.App.Services
{
    public class ClientService
    {
        private readonly IClientStorage _clientStorage;

        public ClientService(IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }

        public void ValidateClient(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.PassportData))
            {
                throw new NoPassportDataException("Клиент не имеет паспортных данных");
            }

            if (client.Age < 18)
            {
                throw new UnderagePeopleException("Несовершеннолетний клиент");
            }
        }

        public Client GetClientById(Guid clientId) 
        {
            var client = _clientStorage.GetById(clientId);
            
            if (client == null) 
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            return client;
        }

        public void AddClient(Client client)
        {
            ValidateClient(client);

            var currency = new Currency { Type = "USD" };

            var defaultAccount = new Account { Currency = currency, Amount = 0 };

            _clientStorage.Add(client);
            _clientStorage.AddAccount(client.Id, defaultAccount);
        }

        public List<Client> FilterClients(Func<Client, bool> filter)
        {
            return _clientStorage.Get(filter);
        }

        public void UpdateClient(Client client)
        {
            if (_clientStorage.GetById(client.Id) == null)
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            _clientStorage.Update(client);
        }

        public void DeleteClient(Guid clientId) 
        {
            var client = _clientStorage.GetById(clientId);

            if (client == null)
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            _clientStorage.Delete(clientId);
        }
       
        public void AddAdditionalAccount(Guid clientId, Account account)
        {
            var client = _clientStorage.GetById(clientId);

            if (client == null)
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            _clientStorage.AddAccount(clientId, account);
        }

        public void DeleteAccount(Guid clientId, Guid accountId) 
        {
            var client = _clientStorage.GetById(clientId);

            if (client == null)
            {
                throw new EntityNotFoundException("Клиент не найден");
            }

            var account = client.Accounts.FirstOrDefault(a => a.Id == accountId);

            if (account == null)
            {
                throw new EntityNotFoundException("Искомый счет не найден");
            }

            _clientStorage.DeleteAccount(clientId, accountId);
        }
    }
}
