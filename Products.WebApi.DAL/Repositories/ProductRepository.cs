using Products.WebApi.DAL.Interfaces;
using Products.WebApi.DAL.Models;

namespace Products.WebApi.DAL.Repositories
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public ProductRepository(ProductContext context) : base(context)
        {
        }
    }
}
