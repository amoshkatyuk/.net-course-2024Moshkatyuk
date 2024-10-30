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

        public async Task<Client> GetByIdAsync(Guid clientId) 
        {
            return await _context.Clients
                .Include(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.Id == clientId);
        }

        public async Task AddAsync(Client client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Client>> GetAsync(Expression<Func<Client, bool>> filter)
        {
            return await _context.Clients
                .Include(c => c.Accounts)
                .Where(filter)
                .ToListAsync();
        }

        public async Task UpdateAsync(Guid clientId, Client client) 
        {
            var existingClient = await GetByIdAsync(clientId);
            _context.Entry(existingClient).CurrentValues.SetValues(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid clientId) 
        {
            var client = await GetByIdAsync(clientId);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }

        public async Task AddAccountAsync(Guid clientId, Account account)
        {
            var client = await GetByIdAsync(clientId);

            if (client == null)
            {
                throw new EntityNotFoundException("Клиент не найден");
            }

            account.ClientId = clientId;
            account.Client = client;

            var existingCurrency = await _context.Currencies.FirstOrDefaultAsync(c => c.Type == account.Currency.Type);
            
            if (existingCurrency == null)
            {
                await _context.Currencies.AddAsync(account.Currency);
            }
            else
            {
                account.CurrencyId = existingCurrency.Id;
                account.Currency = existingCurrency;
            }

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(Guid clientId, Guid accountId) 
        {
            var client = await GetByIdAsync(clientId);
            var account = client.Accounts.FirstOrDefault(a => a.Id == accountId);
            
            if (account != null) 
            {
                client.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<double> GetAverageAgeAsync() 
        {
            return await _context.Clients.AverageAsync(c => c.Age);
        }
    }
}
