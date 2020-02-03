using DataAccess.Repositories;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IMoodRepository MoodRepository { get; }
        public IUserMoodRepository UserMoodRepository { get; }

        Task<int> SaveChangesAsync();
    }
}