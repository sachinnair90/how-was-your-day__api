using DataAccess.Entities;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> AnyUserExists();
        Task<User> GetUserFromCredentials(string email, string password);
    }
}