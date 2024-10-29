using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Exceptions;
using BankSystem.Domain.Models;
using BankSystem.App.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace BankSystem.App.Services
{
    public class ClientService
    {
        private readonly IClientStorage _clientStorage;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public ClientService( IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }

        public async Task ValidateClientAsync(Client client)
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

        public async Task<Client> GetClientByIdAsync(Guid clientId) 
        {
            var client = await _clientStorage.GetByIdAsync(clientId);
            
            if (client == null) 
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            return client;
        }

        public async Task AddClientAsync(Client client)
        {
            await ValidateClientAsync(client);

            var currency = new Currency { Type = "USD" };

            var defaultAccount = new Account { Currency = currency, Amount = 0 };

            await _clientStorage.AddAsync(client);
            await _clientStorage.AddAccountAsync(client.Id, defaultAccount);
        }

        public async Task<List<Client>> FilterClientsAsync(Func<Client, bool> filter)
        {
            return await _clientStorage.GetAsync(filter);
        }

        public async Task UpdateClientAsync(Client client)
        {
            var existingClient = await _clientStorage.GetByIdAsync(client.Id);

            if (existingClient == null)
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            await _clientStorage.UpdateAsync(client.Id, client);
        }

        public async Task DeleteClientAsync(Guid clientId) 
        {
            var client = await _clientStorage.GetByIdAsync(clientId);

            if (client == null)
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            await _clientStorage.DeleteAsync(clientId);
        }
       
        public async Task AddAdditionalAccountAsync(Guid clientId, Account account)
        {
            var client = await _clientStorage.GetByIdAsync(clientId);

            if (client == null)
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            await _clientStorage.AddAccountAsync(clientId, account);
        }

        public async Task DeleteAccountAsync(Guid clientId, Guid accountId) 
        {
            var client = await _clientStorage.GetByIdAsync(clientId);

            if (client == null)
            {
                throw new EntityNotFoundException("Клиент не найден");
            }

            var account = client.Accounts.FirstOrDefault(a => a.Id == accountId);

            if (account == null)
            {
                throw new EntityNotFoundException("Искомый счет не найден");
            }

            await _clientStorage.DeleteAccountAsync(clientId, accountId);
        }

        public async Task<bool> WithdrawFromAccountsAsync(Dictionary<Guid, List<decimal>> withdrawalRequests)
        {
            var tasks = withdrawalRequests.Select(async request =>
            {
                await _semaphore.WaitAsync(); // Ожидание доступа к базе данных
                try
                {
                    bool allWithdrawalsSuccessful = true;
                    foreach (var amount in request.Value)
                    {
                        bool result = await WithdrawAsync(request.Key, amount);
                        if (!result)
                        {
                            allWithdrawalsSuccessful = false;
                        }
                    }
                    return allWithdrawalsSuccessful;
                }
                finally
                {
                    _semaphore.Release(); // Освобождаем семафор
                }
            });

            var results = await Task.WhenAll(tasks);
            return results.All(result => result);
        }

        private async Task<bool> WithdrawAsync(Guid clientId, decimal amount)
        {
            var client = await _clientStorage.GetByIdAsync(clientId);
            if (client == null || client.Accounts == null || !client.Accounts.Any())
            {
                return false;
            }

            var account = client.Accounts.FirstOrDefault();

            if (account == null || account.Amount < amount)
            {
                return false;
            }

            account.Amount -= amount;
            await _clientStorage.UpdateAsync(clientId, client);
            return true;
        }
    }
}
