using AutoFixture;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using System.Security.Claims;

namespace BusinessLogic.Tests
{
    public class LoginService
    {
        private readonly ILoginService loginService;
        private readonly IList<DataAccess.Entities.User> users;
        private readonly ITokenGenerator tokenGenerator;

        public LoginService()
        {
            loginService = SetupData(out IEnumerable<DataAccess.Entities.User> _users);
            users = _users.ToList();
        }

        [Fact]
        public void Authenticate_User_With_Credentials()
        {
            var fixture = new Fixture();
            var _user = users[0];
            var email = _user.Email;
            var password = fixture.Create<string>();

            User user = loginService.Authenticate(email, password);

            User expectedUser = new User
            {
                Id = _user.Id,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                Email = _user.Email,
                Token = tokenGenerator.GetToken(fixture.CreateMany<Claim>().ToArray(), fixture.Create<int>(), fixture.Create<string>())
            };

            user.Should().BeEquivalentTo(expectedUser);
        }
    }
}
