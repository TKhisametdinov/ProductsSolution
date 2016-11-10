using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using NLog;
using Products.WebApi.BL.Interfaces;
using Products.WebApi.DAL.Models;
using WebApi.OutputCache.V2;

namespace Products.WebApi.Controllers
{
    [AutoInvalidateCacheOutput]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public ProductsController( IProductService productService, ILogger logger)
        {
            _productService = productService;
            _logger = logger;
        }

        // GET: api/Products
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productService.GetOrderedProducts();
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await _productService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            try
            {
                if (!await _productService.PutProduct(id, product))
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Put product error: {ex.Message}");
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productService.PostProduct(product);

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            var product = await _productService.DeleteProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}