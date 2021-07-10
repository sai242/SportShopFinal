using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWebService.Services
{
    public interface IRepo<T, K>
    {
        ICollection<T> GetAll();
        T Get(K k);
        T DeleteOrder(K k);
        K Add(T t);
    }
}
