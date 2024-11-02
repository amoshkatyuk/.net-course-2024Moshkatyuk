using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IClientService
    {
        public Task<Client> GetClientByIdAsync(Guid clientId, CancellationToken cancellationToken);
        public Task AddClientAsync(Client client, CancellationToken cancellationToken);
        public Task UpdateClientAsync(Client client, CancellationToken cancellationToken);
        public Task DeleteClientAsync(Guid clientId, CancellationToken cancellationToken);
        public Task<List<Client>> FilterClientsAsync(Expression<Func<Client, bool>> filter, CancellationToken cancellationToken);
    }
}
