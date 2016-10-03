using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Products.WebApi.Interfaces;

namespace Products.WebApi.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private bool _disposed;
        private readonly ProductContext _context;
        internal DbSet<TEntity> DbSet;

        public Repository(ProductContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }
       
        public virtual async Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                var result = orderBy(query).ToList();
                return await Task.FromResult(result);
            }
            return await query.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}