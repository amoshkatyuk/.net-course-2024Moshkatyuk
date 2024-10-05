using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Exceptions;
using BankSystem.Domain.Models;
using BankSystem.Data.Storages;

namespace BankSystem.App.Services
{
    public class ClientService
    {
        private ClientStorage _clientStorage;

        public ClientService()
        {
            _clientStorage = new ClientStorage();
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
            _clientStorage.AddClients(new Dictionary<Client, List<Account>> { { client, new List<Account> { defaultAccount } } });
        }

        public void AddAdditionalAccount(Client client, Account account)
        {
            if (!_clientStorage.ClientExists(client))
            {
                throw new ClientNotFoundException("Искомый клиент не найден");
            }
            _clientStorage.AddAccountToClient(client, account);
        }

        public void UpdateAccount(Client client, Account updatedAccount)
        {
            if (!_clientStorage.ClientExists(client))
            {
                throw new ClientNotFoundException("Искомый клиент не найден");
            }
            _clientStorage.UpdateClientAccount(client, updatedAccount);
        }

        public List<Account> GetAccounts(Client client)
        {
            var accounts = _clientStorage.GetAccounts(client);
            return accounts;
        }

        public List<Client> FilterClients(string name = null, string surname = null, string passportData = null, string telephoneNumber = null, DateTime? birthDateFrom = null, DateTime? birthDateTo = null)
        {
            return _clientStorage.FilterClients(name, surname, passportData, telephoneNumber, birthDateFrom, birthDateTo);
        }
    }
}
