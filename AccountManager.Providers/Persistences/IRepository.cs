using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Providers.Persistences
{
    public interface IRepository<T> where T : IDatabasable
    {
        IEnumerable<T> GetAll();
        T Get(string id);
        bool Exists(string id);
        void Add(T value);
        void Delete(string id);
    }
}
