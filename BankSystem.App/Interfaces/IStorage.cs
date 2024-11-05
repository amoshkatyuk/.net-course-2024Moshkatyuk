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
    public interface IStorage<T>
    {
        public Task AddAsync(T item, CancellationToken cancellationToken);
        public Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<T>> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
        public Task UpdateAsync(Guid id, T item, CancellationToken cancellationToken);
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    }
}
