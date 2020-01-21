using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Infrastructure.Tests
{
    public class TokenGeneratorTests
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly Mock<JwtSecurityTokenHandler> _tokenHandler;

        public TokenGeneratorTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _tokenHandler = fixture.Create<Mock<JwtSecurityTokenHandler>>();
            _tokenGenerator = new TokenGenerator(_tokenHandler.Object);
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

            var result = _tokenGenerator.GetToken(new[] { claim }, fixture.Create<int>(), fixture.Create<string>());

            result.Should().BeEquivalentTo(tokenString);
        }

        [Fact]
        public void Throw_Argument_Null_Exception_If_Claims_Were_Not_Supplied()
        {
            Action action = () => _tokenGenerator.GetToken(null, default, "dummy");

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Throw_Argument_Exception_If_Secret_Was_Not_Supplied()
        {
            Action action = () => _tokenGenerator.GetToken(new[] { new Claim(ClaimTypes.Name, "") }, default, null);

            action.Should().ThrowExactly<ArgumentException>();
        }
    }
}