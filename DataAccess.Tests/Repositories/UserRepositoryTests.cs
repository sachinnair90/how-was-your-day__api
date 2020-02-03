using AutoFixture;
using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;
using System.Net.Mail;
using System;
using DataAccess.Exceptions;
using System.Collections.Generic;
using DataAccess.Entities;
using DataAccess.Repositories;
using AutoFixture.AutoMoq;

namespace DataAccess.Tests
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private AppDBContext _dbContext;
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests()
        {
            _unitOfWork = SetupRepository();
            _userRepository = _unitOfWork.UserRepository;
        }

        [Fact]
        public void Check_If_No_User_Exists()
        {
            _userRepository.AnyUserExists().GetAwaiter().GetResult().Should().BeFalse();
        }

        [Fact]
        public void Check_If_Any_User_Exists()
        {
            AddUsersToDB(out _);

            _userRepository.AnyUserExists().GetAwaiter().GetResult().Should().BeTrue();
        }

        [Fact]
        public void Check_If_Given_User_With_Given_Credentials_Exist()
        {
            // Arrange
            var users = AddUsersToDB(out var defaultPassword);

            // Act
            var user = _userRepository.GetUserFromCredentials(users[0].Email, defaultPassword).GetAwaiter().GetResult();

            // Assert
            user.Should().BeEquivalentTo(users[0]);
        }

        [Fact]
        public void Throw_Argument_Null_Exception_If_User_Credentials_Are_Invalid()
        {
            Action getUser = () => _userRepository.GetUserFromCredentials(null, null).GetAwaiter().GetResult();

            getUser.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Throw_Invalid_User_Password_Exception_If_User_Credentials_Are_Invalid()
        {
            // Arrange
            var users = AddUsersToDB(out _);

            // Act
            Action getUser = () => _userRepository.GetUserFromCredentials(users[0].Email, new Fixture().Create<string>()).GetAwaiter().GetResult();

            // Assert
            getUser.Should().ThrowExactly<InvalidUserPasswordException>();
        }

        [Fact]
        public void Throw_User_Not_Found_Exception_Is_No_User_Is_Found_For_Given_Credentials()
        {
            // Arrange
            AddUsersToDB(out _);

            var fixture = new Fixture();

            // Act
            Action getUser = () => _userRepository.GetUserFromCredentials(fixture.Create<MailAddress>().Address, fixture.Create<string>()).GetAwaiter().GetResult();

            // Assert
            getUser.Should().ThrowExactly<UserNotFoundException>();
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

            var userRepository = new UserRepository(_dbContext, new HashHelpers());
            var moodRepo = fixture.Create<MoodRepository>();
            var userMoodRepository = fixture.Create<UserMoodRepository>();

            return new UnitOfWork(_dbContext, userRepository, moodRepo, userMoodRepository);
        }

        private List<User> AddUsersToDB(out string defaultPassword)
        {
            var fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                        .ForEach(b => fixture.Behaviors.Remove(b));

            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            var defaultUserPassword = fixture.Create<string>();
            defaultPassword = defaultUserPassword;

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
                var hash = hashHelpers.GetNewHash(out var salt, defaultUserPassword);
                x.PasswordHash = hash;
                x.PasswordSalt = salt;
            });

            _unitOfWork.UserRepository.Add(users);

            _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

            return users;
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

        ~UserRepositoryTests()
        {
            Dispose(false);
        }

        #endregion Cleanup
    }
}