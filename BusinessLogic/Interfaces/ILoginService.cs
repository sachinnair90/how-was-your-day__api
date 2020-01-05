using BusinessLogic.DTO;

namespace BusinessLogic.Interfaces
{
    public interface ILoginService
    {
        AuthenticatedUser Authenticate(string email, string password);
    }
}
