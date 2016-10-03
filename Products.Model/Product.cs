using System;

namespace Products.Model
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime LastUpdated { get; set; }
        public byte[] PhotoData { get; set; }
        public int PhotoWidth { get; set; }
        public int PhotoHeight { get; set; }
        public Product()
        {
                
        }

        public Product(int id, string name, byte[] photoData, double price, DateTime lastUpdated, int photoWidth, int photoHeight)
        {
            ID = id;
            Name = name;
            PhotoData = photoData;
            PhotoWidth = photoWidth;
            PhotoHeight = photoHeight;
            Price = price;
            LastUpdated = lastUpdated;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
