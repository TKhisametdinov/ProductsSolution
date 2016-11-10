using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Reflection;
using System.Web.Hosting;
using Products.WebApi.DAL.Models;

namespace Products.WebApi.DAL.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ProductContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ProductContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var students = new List<Product>
            {
                new Product
                {
                    Name = "Apple iPhone 7 32Gb",
                    Price = 300,
                    PhotoData = ImageToByteArray("Apple iPhone 7 32Gb.data"),
                    PhotoHeight = 701,
                    PhotoWidth = 366,
                    LastUpdated = DateTime.Parse("2005-09-01")
                },
                new Product
                {
                    Name = "Xiaomi Redmi Note 3 Pro 32Gb",
                    Price = 300,
                    PhotoData = ImageToByteArray("Xiaomi Redmi Note 3 Pro 32Gb.data"),
                    PhotoHeight = 701,
                    PhotoWidth = 436,
                    LastUpdated = DateTime.Parse("2002-09-01")
                },
                new Product
                {
                    Name = "Samsung Galaxy S7 32Gb",
                    Price = 300,
                    PhotoData = ImageToByteArray("Samsung Galaxy S7 32Gb.data"),
                    PhotoHeight = 556,
                    PhotoWidth = 284,
                    LastUpdated = DateTime.Parse("2003-09-01")
                },
                new Product
                {
                    Name = "Samsung Galaxy S7 Edge 32Gb",
                    Price = 300,
                    PhotoData = ImageToByteArray("Samsung Galaxy S7 Edge 32Gb.data"),
                    PhotoHeight = 556,
                    PhotoWidth = 284,
                    LastUpdated = DateTime.Parse("2002-09-01")
                },
                new Product
                {
                    Name = "Meizu M3 Note 32Gb",
                    Price = 300,
                    PhotoData = ImageToByteArray("Meizu M3 Note 32Gb.data"),
                    PhotoHeight = 701,
                    PhotoWidth = 550,
                    LastUpdated = DateTime.Parse("2002-09-01")
                },
                new Product
                {
                    Name = "Xiaomi Redmi Note 3 Pro 16Gb",
                    Price = 300,
                    PhotoData = ImageToByteArray("Xiaomi Redmi Note 3 Pro 16Gb.data"),
                    PhotoHeight = 701,
                    PhotoWidth = 522,
                    LastUpdated = DateTime.Parse("2001-09-01")
                },
                new Product
                {
                    Name = "Xiaomi Redmi 3",
                    Price = 300,
                    PhotoData = ImageToByteArray("Xiaomi Redmi 3.data"),
                    PhotoHeight = 701,
                    PhotoWidth = 522,
                    LastUpdated = DateTime.Parse("2003-09-01")
                },
                new Product
                {
                    Name = "Meizu Pro 6 32Gb",
                    Price = 300,
                    PhotoData = ImageToByteArray("Meizu Pro 6 32Gb.data"),
                    PhotoHeight = 701,
                    PhotoWidth = 550,
                    LastUpdated = DateTime.Parse("2005-09-01")
                }
            };

            students.ForEach(s => context.Products.Add(s));
            context.SaveChanges();
        }

        public byte[] ImageToByteArray(string filename)
        {
            string assemblyFilePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;
            var assemblyPath = Path.GetDirectoryName(assemblyFilePath);
            var path = $"{assemblyPath}\\Images\\{filename}";

            return File.ReadAllBytes(path);
        }
    }
}
