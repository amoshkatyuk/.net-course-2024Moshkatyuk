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
using System.Linq.Expressions;

namespace BankSystem.App.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientStorage _clientStorage;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public ClientService( IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }

        // на текущий момент метод может быть убран, но для успешного прохождения тестов из более ранних заданий он оставлен
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

        public async Task<Client> GetClientByIdAsync(Guid clientId, CancellationToken cancellationToken) 
        {
            var client = await _clientStorage.GetByIdAsync(clientId, cancellationToken);
            
            if (client == null) 
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            return client;
        }

        public async Task AddClientAsync(Client client, CancellationToken cancellationToken)
        {
            await ValidateClientAsync(client);

            var currency = new Currency { Type = "USD" };

            var defaultAccount = new Account { Currency = currency, Amount = 0 };

            await _clientStorage.AddAsync(client, cancellationToken);
            await _clientStorage.AddAccountAsync(client.Id, defaultAccount, cancellationToken);
        }

        public async Task<List<Client>> FilterClientsAsync(Expression<Func<Client, bool>> filter, CancellationToken cancellationToken)
        {
            return await _clientStorage.GetAsync(filter, cancellationToken);
        }

        public async Task UpdateClientAsync(Client client, CancellationToken cancellationToken)
        {
            var existingClient = await _clientStorage.GetByIdAsync(client.Id, cancellationToken);

            if (existingClient == null)
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            await _clientStorage.UpdateAsync(client.Id, client, cancellationToken);
        }

        public async Task DeleteClientAsync(Guid clientId, CancellationToken cancellationToken) 
        {
            var client = await _clientStorage.GetByIdAsync(clientId, cancellationToken);

            if (client == null)
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            await _clientStorage.DeleteAsync(clientId, cancellationToken);
        }
       
        public async Task AddAdditionalAccountAsync(Guid clientId, Account account, CancellationToken cancellationToken)
        {
            var client = await _clientStorage.GetByIdAsync(clientId, cancellationToken);

            if (client == null)
            {
                throw new EntityNotFoundException("Искомый клиент не найден");
            }

            await _clientStorage.AddAccountAsync(clientId, account, cancellationToken);
        }

        public async Task DeleteAccountAsync(Guid clientId, Guid accountId, CancellationToken cancellationToken) 
        {
            var client = await _clientStorage.GetByIdAsync(clientId, cancellationToken);

            if (client == null)
            {
                throw new EntityNotFoundException("Клиент не найден");
            }

            var account = client.Accounts.FirstOrDefault(a => a.Id == accountId);

            if (account == null)
            {
                throw new EntityNotFoundException("Искомый счет не найден");
            }

            await _clientStorage.DeleteAccountAsync(clientId, accountId, cancellationToken);
        }

        public async Task<bool> WithdrawFromAccountsAsync(Dictionary<Guid, List<decimal>> withdrawalRequests, CancellationToken cancellationToken)
        {
            var tasks = withdrawalRequests.Select(async request =>
            {
                await _semaphore.WaitAsync(cancellationToken);
                try
                {
                    bool allWithdrawalsSuccessful = true;
                    foreach (var amount in request.Value)
                    {
                        bool result = await WithdrawAsync(request.Key, amount, cancellationToken);
                        if (!result)
                        {
                            allWithdrawalsSuccessful = false;
                        }
                    }
                    return allWithdrawalsSuccessful;
                }
                finally
                {
                    _semaphore.Release();
                }
            });

            var results = await Task.WhenAll(tasks);
            return results.All(result => result);
        }

        private async Task<bool> WithdrawAsync(Guid clientId, decimal amount, CancellationToken cancellationToken)
        {
            var client = await _clientStorage.GetByIdAsync(clientId, cancellationToken);
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
            await _clientStorage.UpdateAsync(clientId, client, cancellationToken);
            return true;
        }
    }
}
