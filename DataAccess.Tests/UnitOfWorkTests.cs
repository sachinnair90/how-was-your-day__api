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

            fixture.Register(() => dbContext);

            return fixture.Create<UnitOfWork>();
        }

        #endregion Setup Data
    }
}