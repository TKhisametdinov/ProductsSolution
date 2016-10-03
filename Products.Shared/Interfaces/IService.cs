using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.Shared.Interfaces
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> Get();
        Task<bool> Remove(int id);
        Task<T> Get(int id);
        Task<bool> Update(T entity);
        Task<bool> Add(T entity);
    }
}
