using DataAccess.Repositories;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IMoodRepository MoodRepository { get; }

        Task<int> SaveChangesAsync();
    }
}