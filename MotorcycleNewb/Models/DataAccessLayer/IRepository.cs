using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleNewb.Models.DataAccessLayer
{
    public interface IRepository<T>
    {
        void Add(T element);
        T GetElement(Func<T, bool> func);
        void Remove(T element);
        IEnumerable<T> GetElements(Func<T, bool> func);
        int Count();
    }
}
