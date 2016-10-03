using Products.Shared.Interfaces;

namespace Products.Shared
{
    public class ProductsApiUrlProvider : IProductApiUrlProvider
    {
        private readonly string _baseUrl;

        public ProductsApiUrlProvider(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public string GetBaseServiceUrl()
        {
            return _baseUrl;
        }

        public string GetProductUrl(int id)
        {
            return $"{_baseUrl}/api/Products/{id}";
        }

        public string GetProductsUrl()
        {
            return $"{_baseUrl}/api/Products/";
        }

        public string DeleteProductUrl(int id)
        {
            return $"{_baseUrl}/api/Products/{id}";
        }

        public string CurrentProductUrl(int id)
        {
            return $"{_baseUrl}/api/Products/{id}";
        }
    }
}