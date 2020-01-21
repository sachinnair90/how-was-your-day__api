using AutoFixture;
using AutoFixture.AutoMoq;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Xunit;

namespace Api.Tests
{
    public class MoodControllerTests
    {
        private readonly MoodController controller;
        private Mock<IMoodService> service;

        public MoodControllerTests()
        {
            controller = SetupData();
        }

        [Fact]
        public void Get_All_Moods()
        {
            var fixture = new Fixture();

            var moods = fixture.CreateMany<Mood>();

            service.Setup(x => x.GetAllMoodsAsync()).ReturnsAsync(moods);

            var result = controller.Get().GetAwaiter().GetResult();

            result.Should().BeOfType<OkObjectResult>();
        }

        #region Setup Data

        private MoodController SetupData()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            service = fixture.Freeze<Mock<IMoodService>>();

            fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());

            return fixture.Create<MoodController>();
        }

        #endregion Setup Data
    }
}