using DataAccess.Repositories;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        Task<int> SaveChangesAsync();
    }
}