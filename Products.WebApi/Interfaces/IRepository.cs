using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Products.WebApi.Interfaces
{
    public interface IRepository<TEntity> : IDisposable
    {
        Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            string includeProperties = "");

        Task<TEntity> GetByIdAsync(int id);

        void Add(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}