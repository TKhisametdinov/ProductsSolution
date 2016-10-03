using System;
using System.Collections.Generic;
using System.IO;
using Products.WebApi.Models;

namespace Products.WebApi.DAL
{
    public class ProductInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ProductContext>
    {
        protected override void Seed(ProductContext context)
        {
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
            string path = System.Web.HttpContext.Current.Request.MapPath($"~\\App_Data\\{filename}");

            return File.ReadAllBytes(path);
        }
    }
}