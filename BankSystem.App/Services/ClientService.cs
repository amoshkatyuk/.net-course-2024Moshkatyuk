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

        public void AddClient(Client client)
        {
            ValidateClient(client);

            var defaultAccount = new Account { Currency = "USD", Amount = 0 };

            _clientStorage.Add(client);
            _clientStorage.AddAccount(client, defaultAccount);
        }

        public void DeleteClient(Client client) 
        {
            if (!_clientStorage.ClientExists(client))
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            _clientStorage.Delete(client);
        }

        public void UpdateClient(Client client) 
        {
            if (!_clientStorage.ClientExists(client))
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }
            _clientStorage.Update(client);
        }

        public void AddAdditionalAccount(Client client, Account account)
        {
            if (!_clientStorage.ClientExists(client))
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }
            _clientStorage.AddAccount(client, account);
        }

        public void UpdateAccount(Client client, Account updatedAccount)
        {
            if (!_clientStorage.ClientExists(client))
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }
            _clientStorage.UpdateAccount(client, updatedAccount);
        }

        public List<Account> GetAccounts(Client client)
        {
            var accounts = _clientStorage.GetAccounts(client);
            return accounts;
        }

        public List<Client> FilterClients(Func<Client, bool> filter)
        {
            return _clientStorage.Get(filter);
        }
    }
}
