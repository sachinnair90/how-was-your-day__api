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
            SeedUsers(unitOfWork, hashHelpers, options);
            SeedMoods(unitOfWork, options);

            unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
        }

        private static void SeedMoods(IUnitOfWork unitOfWork, IOptions<Configuration> options)
        {
            if (unitOfWork.MoodRepository.GetCountAsync().GetAwaiter().GetResult() >= options.Value.Moods.Count)
            {
                return;
            }

            options.Value.Moods.ForEach(x => unitOfWork.MoodRepository.Add(new DataAccess.Entities.Mood { Name = x.Name, Description = x.Description }));
        }

        private static void SeedUsers(IUnitOfWork unitOfWork, IHashHelpers hashHelpers, IOptions<Configuration> options)
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
        }
    }
}