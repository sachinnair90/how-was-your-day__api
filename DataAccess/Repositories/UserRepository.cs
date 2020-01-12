using DataAccess.Entities;
using DataAccess.Exceptions;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IHashHelpers hashHelpers;

        public UserRepository(AppDBContext context, IHashHelpers hashHelpers) : base(context)
        {
            this.hashHelpers = hashHelpers;
        }

        public async Task<bool> AnyUserExists()
        {
            return await GetQueryable().AnyAsync();
        }

        public async Task<User> GetUserFromCredentials(string email, string password)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));

            if (password == null) throw new ArgumentNullException(nameof(password));

            var user = await GetQueryable().SingleOrDefaultAsync(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            if (user == null) throw new UserNotFoundException();

            var isPasswordValid = hashHelpers.CompareHash(password, user.PasswordSalt, user.PasswordHash);

            if (!isPasswordValid) throw new InvalidUserPasswordException();

            return user;
        }
    }
}
