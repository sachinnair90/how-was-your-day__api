using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using FluentAssertions;
using Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace Infrastructure.Tests
{
    public class TokenGenerator
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly Mock<JwtSecurityTokenHandler> _tokenHandler;

        public TokenGenerator()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _tokenHandler = fixture.Create<Mock<JwtSecurityTokenHandler>>();
            _tokenGenerator = new Infrastructure.TokenGenerator(_tokenHandler.Object);
        }

        [Fact]
        public void Return_Token_If_Valid_Values_Are_Supplied()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var securityToken = fixture.Create<Mock<SecurityToken>>();
            var tokenString = fixture.Create<string>();

            var claim = new Claim(ClaimTypes.Name, "DummyClaim");

            _tokenHandler.Setup(x => x.CreateToken(It.IsAny<SecurityTokenDescriptor>())).Returns(securityToken.Object);
            _tokenHandler.Setup(x => x.WriteToken(It.IsAny<SecurityToken>())).Returns(tokenString);

            var result = _tokenGenerator.GetToken(new []{ claim }, fixture.Create<int>(), fixture.Create<string>());

            result.Should().BeEquivalentTo(tokenString);
        }
    }
}
