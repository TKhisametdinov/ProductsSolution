using System;
using System.Threading.Tasks;

namespace Products.WebApi.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
