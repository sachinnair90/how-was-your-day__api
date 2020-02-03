using AutoFixture;
using AutoFixture.AutoMoq;
using DataAccess.DTO;
using DataAccess.Entities;
using DataAccess.Repositories;
using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Xunit;

namespace DataAccess.Tests.Repositories
{
    public class UserMoodRepositoryTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserMoodRepository userMoodRepository;
        private AppDBContext _dbContext;

        public UserMoodRepositoryTests()
        {
            _unitOfWork = SetupRepository();
            userMoodRepository = _unitOfWork.UserMoodRepository;
        }

        [Fact]
        public void Get_All_Recorded_Moods_For_The_User()
        {
            var fixture = new Fixture();

            var userMoods = AddDataToDB();

            var user = _unitOfWork.UserRepository.GetAllAsync().GetAwaiter().GetResult().FirstOrDefault();

            var expected = userMoods.Where(x => x.UserId == user.Id);

            var parameters = fixture.Build<FilterMood>()
                .With(x => x.UserId, user.Id)
                .Without(x => x.From)
                .Without(x => x.To)
                .Create();

            var result = userMoodRepository.GetMoodsForUser(parameters).GetAwaiter().GetResult();

            result.Should().BeEquivalentTo(expected, x => x.WithStrictOrdering());
        }

        [Fact]
        public void Filter_Recorded_Moods_For_The_User_From_Given_Date_Including_The_Date()
        {
            var fixture = new Fixture();

            var fromDateTime = DateTime.Now.AddDays(-2);

            var userMoods = AddDataToDB();

            var user = _unitOfWork.UserRepository.GetAllAsync().GetAwaiter().GetResult().FirstOrDefault();

            var expected = userMoods.Where(x => x.UserId == user.Id && x.CreatedDate >= fromDateTime);

            var parameters = fixture.Build<FilterMood>()
                .With(x => x.UserId, user.Id)
                .With(x => x.From, fromDateTime)
                .Without(x => x.To)
                .Create();

            var result = userMoodRepository.GetMoodsForUser(parameters).GetAwaiter().GetResult();

            result.Should().BeEquivalentTo(expected, x => x.WithStrictOrdering());
        }

        [Fact]
        public void Filter_Recorded_Moods_For_The_User_Upto_Given_Date_Including_The_Date()
        {
            var fixture = new Fixture();

            var toDateTime = DateTime.Now.AddDays(-1);

            var userMoods = AddDataToDB();

            var user = _unitOfWork.UserRepository.GetAllAsync().GetAwaiter().GetResult().FirstOrDefault();

            var expected = userMoods.Where(x => x.UserId == user.Id && x.CreatedDate <= toDateTime);

            var parameters = fixture.Build<FilterMood>()
                .With(x => x.UserId, user.Id)
                .With(x => x.To, toDateTime)
                .Without(x => x.From)
                .Create();

            var result = userMoodRepository.GetMoodsForUser(parameters).GetAwaiter().GetResult();

            result.Should().BeEquivalentTo(expected, x => x.WithStrictOrdering());
        }

        [Fact]
        public void Filter_Recorded_Moods_For_The_User_Between_Given_Dates_Including_The_Dates()
        {
            var fixture = new Fixture();

            var fromDateTime = DateTime.Now.AddDays(-2).Date;
            var toDateTime = DateTime.Now.AddDays(-1).Date;

            var setupData = AddDataToDB();

            var user = _unitOfWork.UserRepository.GetAllAsync().GetAwaiter().GetResult().FirstOrDefault();

            var expected = setupData.Where(x => x.UserId == user.Id && x.CreatedDate >= fromDateTime && x.CreatedDate <= toDateTime);

            var parameters = fixture.Build<FilterMood>()
                .With(x => x.UserId, user.Id)
                .With(x => x.From, fromDateTime)
                .With(x => x.To, toDateTime)
                .Create();

            var result = userMoodRepository.GetMoodsForUser(parameters).GetAwaiter().GetResult();

            result.Should().BeEquivalentTo(expected, x => x.WithStrictOrdering());
        }

        #region Data Setup Methods

        private IUnitOfWork SetupRepository()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: nameof(AppDBContext))
                .Options;

            _dbContext = new AppDBContext(options);

            _dbContext.Database.EnsureCreated();

            var userRepo = new UserRepository(_dbContext, fixture.Create<HashHelpers>());
            var moodRepo = new MoodRepository(_dbContext);
            var userMoodRepo = new UserMoodRepository(_dbContext);

            return new UnitOfWork(_dbContext, userRepo, moodRepo, userMoodRepo);
        }

        private List<UserMood> AddDataToDB()
        {
            var fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                        .ForEach(b => fixture.Behaviors.Remove(b));

            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            var defaultUserPassword = fixture.Create<string>();

            var dateTime = fixture.Create<DateTime>().Date;

            var users = fixture.Build<User>()
                .With(x => x.Email, () => fixture.Create<MailAddress>().Address)
                .With(x => x.CreatedAt, dateTime)
                .With(x => x.UpdatedAt, dateTime)
                .Without(x => x.UserMoods)
                .CreateMany().ToList();

            var hashHelpers = new HashHelpers();

            users.ForEach(x =>
            {
                x.PasswordHash = hashHelpers.GetNewHash(out var salt, defaultUserPassword);
                x.PasswordSalt = salt;
            });

            var moods = fixture.CreateMany<Entities.Mood>().ToList();

            _unitOfWork.UserRepository.Add(users);
            _unitOfWork.MoodRepository.Add(moods);

            var userMoods = new List<UserMood>();

            for (var index = 0; index < moods.Count; index++)
            {
                userMoods.Add(new UserMood
                {
                    MoodId = moods[index].Id,
                    UserId = users[0].Id,
                    Comments = fixture.Create<string>(),
                    CreatedDate = DateTime.Now.AddDays(-(index + 1)).Date
                });
            }

            userMoods[0].UserId = users[1].Id;

            _unitOfWork.UserMoodRepository.Add(userMoods);

            _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

            return userMoods;
        }

        #endregion Data Setup Methods

        #region Cleanup

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _dbContext != null)
            {
                _dbContext.Database.EnsureDeleted();
                _dbContext.Dispose();
            }
        }

        ~UserMoodRepositoryTests()
        {
            Dispose(false);
        }

        #endregion Cleanup
    }
}