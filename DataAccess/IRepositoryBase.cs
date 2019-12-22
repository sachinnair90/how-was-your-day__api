using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IRepositoryBase<T> where T:class, new()
    {
        IQueryable<T> GetAll();

        EntityEntry<T> Add(T entity);

        void AddAll(IEnumerable<T> entities);

        EntityEntry<T> Edit(T entity);

        void EditAll(IEnumerable<T> entities);

        EntityEntry<T> Delete(T entity);

        void DeleteAll(IEnumerable<T> entities);

        Task<int> SaveAsync();
    }
}