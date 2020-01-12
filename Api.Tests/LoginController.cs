using AutoFixture;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Tests
{
    public class LoginController
    {
        private Mock<ILoginService> _service;
        private Controllers.LoginController _controller;

        public LoginController()
        {
            SetupData();
        }

        [Fact]
        public void Should_Login_User_With_Valid_Credentials()
        {
            var fixture = new Fixture();
            var authenticatedUser = fixture.Create<AuthenticatedUser>();
            
            _service.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(authenticatedUser);

            var result = _controller.Login(fixture.Create<string>(), fixture.Create<string>()).GetAwaiter().GetResult() as OkObjectResult;

            (result.Value as AuthenticatedUser).Should().BeEquivalentTo(authenticatedUser);
        }

        [Fact]
        public void Return_Unauthorized_Result_If_Invalid_Credentials_Are_Supplied()
        {
            var fixture = new Fixture();

            _service.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<InvalidCredentialsException>();

            var result = _controller.Login(fixture.Create<string>(), fixture.Create<string>()).GetAwaiter().GetResult();

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void Return_Not_Found_Result_If_User_Is_Not_Found()
        {
            var fixture = new Fixture();

            _service.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<UserNotFoundException>();

            var result = _controller.Login(fixture.Create<string>(), fixture.Create<string>()).GetAwaiter().GetResult();

            result.Should().BeOfType<NotFoundResult>();
        }

        #region Setup Data
        private void SetupData()
        {
            _service = new Mock<ILoginService>();

            _controller = new Controllers.LoginController(_service.Object);
        } 
        #endregion
    }
}
