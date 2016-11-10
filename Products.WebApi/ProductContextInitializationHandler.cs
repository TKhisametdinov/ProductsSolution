using System.Data.Entity;
using Products.WebApi.DAL;
using Products.WebApi.DAL.Migrations;

namespace Products.WebApi
{
    public class ProductContextInitializationHandler
    {
        /// <summary>
        /// Set custom initializer and runs it.
        /// </summary>
        public static void Initialize()
        {
            Database.SetInitializer(new CheckAndMigrateDatabaseToLatestVersion<ProductContext, Configuration>());
            using (var context = new ProductContext())
            {
                context.Database.Initialize(false);
            }
        }
    }
}