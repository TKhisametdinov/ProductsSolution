using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Model;
using Products.Shared.Interfaces;
using Newtonsoft.Json;

namespace Products.BusinessLayer
{
    public class ProductService : IService<Product>
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IProductApiUrlProvider _productApiUrlProvider;

        public ProductService(IHttpClientHelper httpClientHelper, IProductApiUrlProvider productApiUrlProvider)
        {
            _httpClientHelper = httpClientHelper;
            _productApiUrlProvider = productApiUrlProvider;
        }

        public async Task<IEnumerable<Product>> Get()
        {
            var url = _productApiUrlProvider.GetProductsUrl();
            var response = await _httpClientHelper.GetAsync(url);
            var data = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(data);

            return products;
        }

        public async Task<bool> Remove(int id)
        {
            var url = _productApiUrlProvider.DeleteProductUrl(id);
            var response = await _httpClientHelper.DeleteAsync(url);

            return response.IsSuccessStatusCode;
        }

        public async Task<Product> Get(int id)
        {
            var url = _productApiUrlProvider.GetProductUrl(id);
            var response = await _httpClientHelper.GetAsync(url);
            var data = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(data);

            return product;
        }

        public async Task<bool> Update(Product entity)
        {
            var url = _productApiUrlProvider.CurrentProductUrl(entity.ID);
            var bodyContent = JsonConvert.SerializeObject(entity);
            var response = await _httpClientHelper.PutAsync(url, bodyContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Add(Product entity)
        {
            var url = _productApiUrlProvider.GetProductsUrl();
            var bodyContent = JsonConvert.SerializeObject(entity);
            var response = await _httpClientHelper.PostAsync(url, bodyContent);

            return response.IsSuccessStatusCode;
        }
    }
}
