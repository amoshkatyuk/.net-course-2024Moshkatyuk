using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IStorage<T>
    {
        public Task AddAsync(T item);
        public Task<T> GetByIdAsync(Guid id);
        public Task<List<T>> GetAsync(Expression<Func<T, bool>> filter);
        public Task UpdateAsync(Guid id, T item);
        public Task DeleteAsync(Guid id);

    }
}
