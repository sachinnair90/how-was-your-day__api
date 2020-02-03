using BusinessLogic.DTO;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAuthenticateService
    {
        Task<AuthenticatedUser> Authenticate(string email, string password);
    }
}
