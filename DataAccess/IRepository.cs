using System.Collections.Generic;

namespace DataAccess
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        IEnumerable<TEntity> GetAll();

        TEntity Add(TEntity entity);

        void Add(IEnumerable<TEntity> entities);

        TEntity Edit(TEntity entity);

        void Edit(IEnumerable<TEntity> entities);

        TEntity Delete(TEntity entity);

        void Delete(IEnumerable<TEntity> entities);
    }
}