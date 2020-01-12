using BusinessLogic.DTO;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ILoginService
    {
        Task<AuthenticatedUser> Authenticate(string email, string password);
    }
}
