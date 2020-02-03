using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Repositories;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BusinessLogic.Tests
{
    public class MoodServiceTests
    {
        private readonly IMoodService moodService;
        private Mock<IMoodRepository> moodRepoMock;
        private Mock<IUserMoodRepository> userMoodRepoMock;
        private Mock<IUnitOfWork> uow;
        private IMapper mapper;

        public MoodServiceTests()
        {
            moodService = SetupData();
        }

        [Fact]
        public void Return_All_Available_Moods()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var expected = fixture.CreateMany<DataAccess.Entities.Mood>();

            moodRepoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

            var moods = moodService.GetAllMoodsAsync().GetAwaiter().GetResult();

            moods.Should().BeEquivalentTo(expected, x => x.WithStrictOrdering());
        }

        [Fact]
        public void Adds_The_Given_Mood_Successfully()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var paramter = fixture.Create<UserMood>();

            userMoodRepoMock.Setup(x => x.Add(It.IsAny<DataAccess.Entities.UserMood>()));

            uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = moodService.AddMoodForUser(paramter).GetAwaiter().GetResult();

            result.Should().Be(true);
        }

        [Fact]
        public void Get_Moods_For_User()
        {
            var fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));

            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var paramter = fixture.Create<FilterMoodParameter>();

            var responseFromRepo = fixture.Build<DataAccess.Entities.UserMood>()
                .With(x => x.Mood, fixture.Create<DataAccess.Entities.Mood>())
                .CreateMany();

            var expected = mapper.Map<IEnumerable<DataAccess.Entities.UserMood>, IEnumerable<UserMoodDetails>>(responseFromRepo);

            userMoodRepoMock.Setup(x => x.GetMoodsForUser(It.IsAny<DataAccess.DTO.FilterMood>())).
                ReturnsAsync(responseFromRepo);

            var result = moodService.GetMoodsForUser(paramter).GetAwaiter().GetResult();

            result.Should().BeEquivalentTo(expected, x => x.WithStrictOrdering());
        }

        #region Setup Data

        private IMoodService SetupData()
        {
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new DataMapper()));

            mapper = mockMapper.CreateMapper();

            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            userMoodRepoMock = fixture.Freeze<Mock<IUserMoodRepository>>();

            moodRepoMock = fixture.Freeze<Mock<IMoodRepository>>();

            uow = fixture.Create<Mock<IUnitOfWork>>();

            uow.SetupGet(x => x.MoodRepository).Returns(moodRepoMock.Object);
            uow.SetupGet(x => x.UserMoodRepository).Returns(userMoodRepoMock.Object);

            return new MoodService(uow.Object, mapper);
        }

        #endregion Setup Data
    }
}