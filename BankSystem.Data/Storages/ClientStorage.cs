using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class ClientStorage : IClientStorage
    {
        private readonly BankSystemDbContext _context;

        public ClientStorage(BankSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetByIdAsync(Guid clientId, CancellationToken cancellationToken) 
        {
            return await _context.Clients
                .Include(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.Id == clientId, cancellationToken);
        }

        public async Task AddAsync(Client client, CancellationToken cancellationToken)
        {
            await _context.Clients.AddAsync(client, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Client>> GetAsync(Expression<Func<Client, bool>> filter, CancellationToken cancellationToken)
        {
            return await _context.Clients
                .Include(c => c.Accounts)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Guid clientId, Client client, CancellationToken cancellationToken) 
        {
            var existingClient = await GetByIdAsync(clientId, cancellationToken);
            _context.Entry(existingClient).CurrentValues.SetValues(client);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid clientId, CancellationToken cancellationToken) 
        {
            var client = await GetByIdAsync(clientId, cancellationToken);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAccountAsync(Guid clientId, Account account, CancellationToken cancellationToken)
        {
            var client = await GetByIdAsync(clientId, cancellationToken);

            if (client == null)
            {
                throw new EntityNotFoundException("Клиент не найден");
            }

            account.ClientId = clientId;
            account.Client = client;

            var existingCurrency = await _context.Currencies.FirstOrDefaultAsync(c => c.Type == account.Currency.Type, cancellationToken);
            
            if (existingCurrency == null)
            {
                await _context.Currencies.AddAsync(account.Currency, cancellationToken);
            }
            else
            {
                account.CurrencyId = existingCurrency.Id;
                account.Currency = existingCurrency;
            }

            await _context.Accounts.AddAsync(account, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAccountAsync(Guid clientId, Guid accountId, CancellationToken cancellationToken) 
        {
            var client = await GetByIdAsync(clientId, cancellationToken);
            var account = client.Accounts.FirstOrDefault(a => a.Id == accountId);
            
            if (account != null) 
            {
                client.Accounts.Remove(account);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<double> GetAverageAgeAsync(CancellationToken cancellationToken) 
        {
            return await _context.Clients.AverageAsync(c => c.Age, cancellationToken);
        }
    }
}
