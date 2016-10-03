using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Products.WebApi.Interfaces;
using Products.WebApi.Models;
using WebApi.OutputCache.V2;

namespace Products.WebApi.Controllers
{
    [AutoInvalidateCacheOutput]
    public class ProductsController : ApiController
    {
        private readonly IRepository<Product> _repository;

        public ProductsController(IRepository<Product> repository)
        {
            _repository = repository;
        }

        // GET: api/Products
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _repository.GetAsync(orderBy: q => q.OrderBy(d => d.Name));
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await _repository.GetByIdAsync(id);
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

            if (id != product.ID)
            {
                return BadRequest();
            }

            _repository.Update(product);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(id))
                {
                    return NotFound();
                }
                throw;
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

            _repository.Add(product);
            await _repository.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.ID }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _repository.Delete(product);
            await _repository.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task<bool> ProductExists(int id)
        {
            var products = await _repository.GetAsync(x => x.ID == id);
            return products.Any();
        }
    }
}