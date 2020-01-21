using Api.Controllers;
using Api.Parameters;
using AutoFixture;
using AutoFixture.AutoMoq;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Xunit;

namespace Api.Tests
{
    public class LoginControllerTests
    {
        private Mock<ILoginService> _service;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _controller = SetupData();
        }

        [Fact]
        public void Should_Login_User_With_Valid_Credentials()
        {
            var fixture = new Fixture();
            var authenticatedUser = fixture.Create<AuthenticatedUser>();

            _service.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(authenticatedUser);

            var result = _controller.Login(fixture.Create<UserAuthenticationParameters>()).GetAwaiter().GetResult() as OkObjectResult;

            (result.Value as AuthenticatedUser)?.Should().BeEquivalentTo(authenticatedUser);
        }

        [Fact]
        public void Return_Unauthorized_Result_If_Invalid_Credentials_Are_Supplied()
        {
            var fixture = new Fixture();

            _service.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<InvalidCredentialsException>();

            var result = _controller.Login(fixture.Create<UserAuthenticationParameters>()).GetAwaiter().GetResult();

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void Return_Not_Found_Result_If_User_Is_Not_Found()
        {
            var fixture = new Fixture();

            _service.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<UserNotFoundException>();

            var result = _controller.Login(fixture.Create<UserAuthenticationParameters>()).GetAwaiter().GetResult();

            result.Should().BeOfType<NotFoundResult>();
        }

        #region Setup Data

        private LoginController SetupData()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            _service = fixture.Freeze<Mock<ILoginService>>();

            fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());

            return fixture.Create<LoginController>();
        }

        #endregion Setup Data
    }
}