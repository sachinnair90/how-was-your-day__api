using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Infrastructure
{
    public static class DBInitializer
    {
        public static void Initialize(IUserRepository userRepository)
        {
            if(userRepository.GetAll().Any())
            {
                return;
            }

            var defaultPassword = "newPassword";

            byte[] passwordHash, passwordSalt;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(defaultPassword));
            }

            var users = new User[]
            {
                new User { FirstName = "Sachin", LastName = "Nair", Email = "sachin.nair@devon.nl", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, PasswordHash = passwordHash, PasswordSalt = passwordSalt }
            };

            userRepository.AddAll(users);

            var saveResult = userRepository.SaveAsync().Result;            
        }
    }
}
