using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using Products.WebApi.BL.Interfaces;
using Products.WebApi.DAL.Interfaces;
using Products.WebApi.DAL.Models;

namespace Products.WebApi.BL
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ProductService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetOrderedProducts()
        {
            return await _unitOfWork.ProductRepository.GetAsync(orderBy: q => q.OrderBy(d => d.Name));
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _unitOfWork.ProductRepository.GetByIdAsync(id);
        }

        public async Task<bool> PutProduct(int id, Product product)
        {
            _unitOfWork.ProductRepository.Update(product);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var products = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == id);
                if (products.Any())
                {
                    return false;
                }
                _logger.Error(ex, $"Product update error: {ex.Message}");
                throw;
            }
            return true;
        }

        public async Task PostProduct(Product product)
        {
            _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Product> DeleteProduct(int id)
        {
            Product product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                return null;
            }

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return product;
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}
