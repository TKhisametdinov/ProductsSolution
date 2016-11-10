using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Products.WebApi.DAL.Models;

namespace Products.WebApi.BL.Interfaces
{
    public interface IProductService: IDisposable
    {
        Task<IEnumerable<Product>> GetOrderedProducts();
        Task<Product> GetProduct(int id);
        Task<bool> PutProduct(int id, Product product);
        Task PostProduct(Product product);
        Task<Product> DeleteProduct(int id);
    }
}
