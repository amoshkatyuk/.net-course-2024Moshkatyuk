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
        public List<T> Get(string name, string surname, string passportData, DateTime? birthDateFrom, DateTime? birthDateTo);
        public void Update(T item);
        public void Delete(T item);

    }
}
