using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IClientStorage : IStorage<Client>
    {
        public Task AddAccountAsync(Guid clientId, Account account, CancellationToken cancellationToken);
        public Task DeleteAccountAsync(Guid clientId, Guid accountId, CancellationToken cancellationToken);
    }
}
