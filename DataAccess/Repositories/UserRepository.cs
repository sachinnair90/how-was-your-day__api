using DataAccess.Entities;
using DataAccess.Exceptions;
using Infrastructure.Interfaces;
using System;
using System.Linq;

namespace DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IHashHelpers hashHelpers;

        public UserRepository(AppDBContext context, IHashHelpers hashHelpers) : base(context)
        {
            this.hashHelpers = hashHelpers;
        }

        public bool AnyUserExists()
        {
            return GetQueryable().Any();
        }

        public User GetUserFromCredentials(string email, string password)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));

            if (password == null) throw new ArgumentNullException(nameof(password));

            var user = GetQueryable().SingleOrDefault(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            if (user == null) throw new UserNotFoundException();

            var isPasswordValid = hashHelpers.CompareHash(password, user.PasswordSalt, user.PasswordHash);

            if (!isPasswordValid) throw new InvalidUserPasswordException();

            return user;
        }
    }
}
