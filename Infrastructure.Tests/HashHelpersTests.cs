using AutoFixture;
using System.Security.Cryptography;
using System.Text;
using Xunit;
using FluentAssertions;
using Infrastructure.Interfaces;

namespace Infrastructure.Tests
{
    public class HashHelpersTests
    {
        private readonly IHashHelpers hashHelpers;

        public HashHelpersTests()
        {
            hashHelpers = new HashHelpers();
        }

        [Fact]
        public void Generate_New_Hash_And_Salt()
        {
            var stringToBeHashed = new Fixture().Create<string>();

            byte[] hash = hashHelpers.GetNewHash(out var salt, stringToBeHashed);

            var hmac = new HMACSHA512(salt);

            var expectedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToBeHashed));

            hmac.Dispose();

            hash.Should().BeEquivalentTo(expectedHash);
        }

        [Fact]
        public void Validate_Given_String_And_Hash_With_Given_Salt()
        {
            var givenString = new Fixture().Create<string>();

            var hmac = new HMACSHA512();
            var salt = hmac.Key;

            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(givenString));

            hmac.Dispose();

            bool isValidHash = hashHelpers.CompareHash(givenString, salt, hash);

            isValidHash.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_Validation_Given_String_And_Hash_With_Given_Salt()
        {
            var givenString = new Fixture().Create<string>();

            var hmac = new HMACSHA512();
            var salt = hmac.Key;

            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(givenString));

            hmac.Dispose();

            bool isValidHash = hashHelpers.CompareHash(string.Concat(givenString, char.MinValue), salt, hash);

            isValidHash.Should().BeFalse();
        }
    }
}