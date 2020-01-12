using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]UserAuthenticationParameters authenticationParameters)
        {
            IActionResult result;
            try
            {
                result = Ok(await _loginService.Authenticate(authenticationParameters.Email, authenticationParameters.Password).ConfigureAwait(false));
            }
            catch (InvalidCredentialsException)
            {
                result = Unauthorized();
            }
            catch (UserNotFoundException)
            {
                result = NotFound();
            }

            return result;
        }
    }
}