using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T:class, new()
    {
        private readonly DbContext context;
        public RepositoryBase(DbContext context)
        {
            this.context = context;
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }

        public EntityEntry<T> Add(T entity)
        {
            return context.Add(entity);
        }

        public void AddAll(IEnumerable<T> entities)
        {
            context.AddRange(entities);
        }

        public EntityEntry<T> Edit(T entity)
        {
            return context.Update(entity);
        }

        public void EditAll(IEnumerable<T> entities)
        {
            context.UpdateRange(entities);
        }

        public EntityEntry<T> Delete(T entity)
        {
            return context.Remove(entity);
        }

        public void DeleteAll(IEnumerable<T> entities)
        {
            context.RemoveRange(entities);
        }

        public Task<int> SaveAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
