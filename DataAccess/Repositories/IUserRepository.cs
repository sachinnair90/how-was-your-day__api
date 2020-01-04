using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool AnyUserExists();
        User GetUserFromCredentials(string email, string password);
    }
}