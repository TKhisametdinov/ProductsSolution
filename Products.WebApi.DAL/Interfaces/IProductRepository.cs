using Products.WebApi.DAL.Models;

namespace Products.WebApi.DAL.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        //Todo: Pagination request
    }
}