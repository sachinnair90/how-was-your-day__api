using System.Security.Claims;

namespace Infrastructure.Interfaces
{
    public interface ITokenGenerator
    {
        string GetToken(Claim[] claim, int maxPasswordExpiryDays, string secret);
    }
}
