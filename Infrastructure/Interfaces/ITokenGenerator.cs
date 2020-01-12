using System.Security.Claims;

namespace Infrastructure.Interfaces
{
    public interface ITokenGenerator
    {
        string GetToken(Claim[] claim, int expiryDays, string key);
    }
}
