using AutoFixture;
using Xunit;
using FluentAssertions;
using System.Security.Claims;
using DataAccess;
using BusinessLogic.Interfaces;
using BusinessLogic.DTO;
using AutoMapper;
using Moq;
using DataAccess.Repositories;
using Infrastructure.Interfaces;
using Infrastructure;
using Microsoft.Extensions.Options;
using AutoFixture.AutoMoq;
using System;
using DataAccess.Exceptions;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Tests
{
    public class LoginService
    {
        [Fact]
        public void Authenticate_User_With_Credentials()
        {
            var fixtureMock = new Fixture().Customize(new AutoMoqCustomization());
            var user = fixtureMock.Create<DataAccess.Entities.User>();
            var token = fixtureMock.Create<string>();

            var repoMock = fixtureMock.Create<Mock<IUserRepository>>();

            repoMock.Setup(x => x.GetUserFromCredentials(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);

            var loginService = SetupData(repoMock, token);

            var fixture = new Fixture();
            var email = user.Email;
            var password = fixture.Create<string>();

            var authenticatedUser = loginService.Authenticate(email, password).GetAwaiter().GetResult();

            var expectedUser = new AuthenticatedUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = token
            };

            authenticatedUser.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public void Should_Throw_Exception_For_User_With_Invalid_Credentials()
        {
            var fixtureMock = new Fixture().Customize(new AutoMoqCustomization());
            var user = fixtureMock.Create<DataAccess.Entities.User>();
            var token = fixtureMock.Create<string>();

            var repoMock = fixtureMock.Create<Mock<IUserRepository>>();

            repoMock.Setup(x => x.GetUserFromCredentials(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new InvalidUserPasswordException());

            var loginService = SetupData(repoMock, token);

            var fixture = new Fixture();
            var email = user.Email;
            var password = fixture.Create<string>();

            Action action = () => loginService.Authenticate(email, password).GetAwaiter().GetResult();

            action.Should().ThrowExactly<InvalidCredentialsException>();
        }

        [Fact]
        public void Should_Throw_Exception_For_Non_Existent_Email()
        {
            var fixtureMock = new Fixture().Customize(new AutoMoqCustomization());
            var user = fixtureMock.Create<DataAccess.Entities.User>();
            var token = fixtureMock.Create<string>();

            var repoMock = fixtureMock.Create<Mock<IUserRepository>>();

            repoMock.Setup(x => x.GetUserFromCredentials(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new DataAccess.Exceptions.UserNotFoundException());

            var loginService = SetupData(repoMock, token);

            var fixture = new Fixture();
            var email = user.Email;
            var password = fixture.Create<string>();

            Action action = () => loginService.Authenticate(email, password).GetAwaiter().GetResult();

            action.Should().ThrowExactly<Exceptions.UserNotFoundException>();
        }

        private ILoginService SetupData(Mock<IUserRepository> repoMock, string token = null)
        {
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new DataMapper()));

            var mapper = mockMapper.CreateMapper();

            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var tokenGeneratorMock = fixture.Create<Mock<ITokenGenerator>>();
            tokenGeneratorMock.Setup(x => x.GetToken(It.IsAny<Claim[]>(), It.IsAny<int>(), It.IsAny<string>())).Returns(token);

            fixture.Register(() => repoMock.Object);
            fixture.Register(() => tokenGeneratorMock.Object);

            var uow = fixture.Create<UnitOfWork>();

            var options = Options.Create(fixture.Create<Configuration>());

            return new BusinessLogic.LoginService(uow, mapper, tokenGeneratorMock.Object, options);
        }
    }
}
