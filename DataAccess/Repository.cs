using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly AppDBContext _context;

        public Repository(AppDBContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var entities = await GetEnitity().AsNoTracking().ToListAsync();

            return entities.AsEnumerable();
        }

        public TEntity Add(TEntity entity)
        {
            return _context.Add(entity).Entity;
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            _context.AddRange(entities);
        }

        public TEntity Edit(TEntity entity)
        {
            return _context.Update(entity).Entity;
        }

        public void Edit(IEnumerable<TEntity> entities)
        {
            _context.UpdateRange(entities);
        }

        public TEntity Delete(TEntity entity)
        {
            return _context.Remove(entity).Entity;
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            _context.RemoveRange(entities);
        }

        protected IQueryable<TEntity> GetQueryable()
        {
            return GetEnitity().AsQueryable();
        }

        public Task<int> GetCountAsync()
        {
            return GetEnitity().CountAsync();
        }

        private DbSet<TEntity> GetEnitity()
        {
            return _context.Set<TEntity>();
        }
    }
}