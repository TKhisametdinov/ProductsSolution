using System;
using System.Data.SqlTypes;

namespace Products.WebApi.DAL.Models
{
    public class Product
    {
        private DateTime _lastUpdated;
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] PhotoData { get; set; }
        public int PhotoWidth { get; set; }
        public int PhotoHeight { get; set; }
        public double Price { get; set; }

        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
            set {
                _lastUpdated = value >= SqlDateTime.MinValue.Value ? value : SqlDateTime.MinValue.Value;
            }
        }
    }
}
