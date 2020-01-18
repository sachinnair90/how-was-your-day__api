using DataAccess;
using DataAccess.Entities;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System;

namespace Api.Infrastructure
{
    public static class DBInitializer
    {
        public static void Initialize(IUnitOfWork unitOfWork, IHashHelpers hashHelpers, IOptions<Configuration> options)
        {
            if (unitOfWork.UserRepository.AnyUserExists().GetAwaiter().GetResult())
            {
                return;
            }

            var passwordHash = hashHelpers.GetNewHash(out var salt, options.Value.Security.DefaultPassword);

            var users = new User[]
            {
                new User {
                    FirstName = "Sachin",
                    LastName = "Nair",
                    Email = "sachin.nair@devon.nl",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    PasswordHash = passwordHash,
                    PasswordSalt = salt
                }
            };

            unitOfWork.UserRepository.Add(users);

            unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
