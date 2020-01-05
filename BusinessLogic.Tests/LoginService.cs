using AutoFixture;
using System.Linq;
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

namespace BusinessLogic.Tests
{
    public class LoginService
    {
        public LoginService() { }

        [Fact]
        public void Authenticate_User_With_Credentials()
        {
            var loginService = SetupData(out DataAccess.Entities.User user, out var token);

            var fixture = new Fixture();
            var email = user.Email;
            var password = fixture.Create<string>();

            AuthenticatedUser authenticatedUser = loginService.Authenticate(email, password);

            AuthenticatedUser expectedUser = new AuthenticatedUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = token
            };

            authenticatedUser.Should().BeEquivalentTo(expectedUser);
        }

        private ILoginService SetupData(out DataAccess.Entities.User user, out string token)
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DataMapper());
            });

            var mapper = mockMapper.CreateMapper();

            var fixture = new Fixture();

            user = fixture.Create<DataAccess.Entities.User>();
            token = fixture.Create<string>();

            var repoMock = fixture.Create<Mock<UserRepository>>();
            var tokenGeneratorMock = fixture.Create<Mock<TokenGenerator>>();

            repoMock.Setup(x => x.GetUserFromCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns(user);
            tokenGeneratorMock.Setup(x => x.GetToken(It.IsAny<Claim[]>(), It.IsAny<int>(), It.IsAny<string>())).Returns(token);

            fixture.Register<IUserRepository>(() => repoMock.Object);
            fixture.Register<ITokenGenerator>(() => tokenGeneratorMock.Object);

            var uow = fixture.Create<IUnitOfWork>();

            var _loginService = new BusinessLogic.LoginService(uow, mapper, tokenGeneratorMock.Object, fixture.Create<IOptions<Configuration>>());

            return _loginService;
        }
    }
}
