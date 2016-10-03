using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Products.WebApi.Models;

namespace Products.WebApi.DAL
{
    public class ProductContext : DbContext
    {

        public ProductContext(string connectionStringName) : base(connectionStringName)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}