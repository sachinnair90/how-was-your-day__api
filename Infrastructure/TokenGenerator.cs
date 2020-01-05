using Infrastructure.Interfaces;
using System.Security.Claims;

namespace Infrastructure
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GetToken(Claim[] claims, int maxPasswordExpiryDays, string secret)
        {
            throw new System.NotImplementedException();
        }
    }
}
