using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDBContext context): base(context)
        {

        }
    }
}
