using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        TEntity Add(TEntity entity);

        void Add(IEnumerable<TEntity> entities);

        TEntity Edit(TEntity entity);

        void Edit(IEnumerable<TEntity> entities);

        TEntity Delete(TEntity entity);

        Task<int> GetCountAsync();
    }
}