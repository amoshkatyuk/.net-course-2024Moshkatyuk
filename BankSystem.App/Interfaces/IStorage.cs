using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IStorage<T>
    {
        public void Add(T item);
        public T GetById(Guid id);
        public List<T> Get(Func<T, bool> filter);
        public void Update(Guid id, T item);
        public void Delete(Guid id);

    }
}
