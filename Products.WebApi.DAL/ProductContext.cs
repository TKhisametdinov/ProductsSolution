using System.Data.Entity;
using Products.WebApi.DAL.Models;

namespace Products.WebApi.DAL
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("ProductStoreDbConnectionString")
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}