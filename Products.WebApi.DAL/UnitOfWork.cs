using System.Threading.Tasks;
using Products.WebApi.DAL.Interfaces;

namespace Products.WebApi.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductContext _context;

        public IProductRepository ProductRepository { get; }

        public UnitOfWork(
            ProductContext context,
            IProductRepository productRepository)
        {
            _context = context;
            ProductRepository = productRepository;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
