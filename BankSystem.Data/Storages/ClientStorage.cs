using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Client GetById(Guid clientId) 
        {
            return _context.Clients.Include(c => c.Accounts)
                .FirstOrDefault(c => c.Id == clientId);
        }

        public void Add(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public List<Client> Get(Func<Client, bool> filter)
        {
            return _context.Clients
                .Include(c => c.Accounts)
                .Where(filter)
                .ToList();
        }

        public void Update(Client client) 
        {
            var existingClient = GetById(client.Id);
            _context.Entry(existingClient).CurrentValues.SetValues(client);
            _context.SaveChanges();
        }

        public void Delete(Guid clientId) 
        {
            var client = GetById(clientId);
            _context.Clients.Remove(client);
            _context.SaveChanges();
        }

        public void AddAccount(Guid clientId, Account account)
        {
            var client = GetById(clientId);

            if (client == null)
            {
                throw new EntityNotFoundException("Клиент не найден");
            }

            account.ClientId = clientId;
            account.Client = client;

            var existingCurrency = _context.Currencies.FirstOrDefault(c => c.Type == account.Currency.Type);
            
            if (existingCurrency == null)
            {
                _context.Currencies.Add(account.Currency);
            }
            else
            {
                account.CurrencyId = existingCurrency.Id;
                account.Currency = existingCurrency;
            }

            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        public void DeleteAccount(Guid clientId, Guid accountId) 
        {
            var client = GetById(clientId);
            var account = client.Accounts.FirstOrDefault(a => a.Id == accountId);
            
            if(account != null) 
            {
                client.Accounts.Remove(account);
                _context.SaveChanges();
            }
        }  
    }
}
