using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public interface IRepo<T, K>
    {
        public Task<ICollection<T>> GetAll();
        Task<T> DeleteOrder(K k);
        Task<T> Get(K k);
        Task<K> Add(T t);
    }
}
