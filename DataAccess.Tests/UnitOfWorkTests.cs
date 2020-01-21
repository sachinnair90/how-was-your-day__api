using AutoFixture;
using AutoFixture.AutoMoq;
using DataAccess.Entities;
using DataAccess.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DataAccess.Tests
{
    public class UnitOfWorkTests
    {
        private readonly IUnitOfWork unitOfWork;
        private Mock<IUserRepository> userRepository;
        private Mock<IMoodRepository> moodRepository;

        public UnitOfWorkTests()
        {
            unitOfWork = SetupData();
        }

        [Fact]
        public void User_Repository_Is_Available()
        {
            unitOfWork.UserRepository.Should().BeAssignableTo<IRepository<User>>().Which.Should().NotBeNull();
        }

        [Fact]
        public void Mood_Repository_Is_Available()
        {
            unitOfWork.MoodRepository.Should().BeAssignableTo<IRepository<Mood>>().Which.Should().NotBeNull();
        }

        #region Setup Data
        private IUnitOfWork SetupData()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: nameof(AppDBContext))
                .Options;

            var dbContext = new AppDBContext(options);

            dbContext.Database.EnsureCreated();

            userRepository = fixture.Create<Mock<IUserRepository>>();
            moodRepository = fixture.Create<Mock<IMoodRepository>>();

            return new UnitOfWork(dbContext, userRepository.Object, moodRepository.Object);
        }
        #endregion
    }
}
