using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace BusinessLogic.Tests
{
    public class MoodServiceTests
    {
        private readonly IMoodService moodService;
        private Mock<IMoodRepository> repoMock;

        public MoodServiceTests()
        {
            moodService = SetupData();
        }

        [Fact]
        public void Return_All_Available_Moods()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var expected = fixture.CreateMany<DataAccess.Entities.Mood>();

            repoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

            var moods = moodService.GetAllMoodsAsync().GetAwaiter().GetResult();

            moods.Should().BeEquivalentTo(expected, x => x.WithStrictOrdering());
        }

        #region Setup Data

        private IMoodService SetupData()
        {
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new DataMapper()));

            var mapper = mockMapper.CreateMapper();

            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            repoMock = fixture.Freeze<Mock<IMoodRepository>>();

            var uow = fixture.Create<UnitOfWork>();

            return new MoodService(uow, mapper);
        }

        #endregion Setup Data
    }
}