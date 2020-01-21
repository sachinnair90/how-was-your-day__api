using Api.Parameters;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController, Authorize]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [SwaggerOperation(
            Summary = "Login user using credentials",
            Description = "Login user unauthenticated user using credentials",
            OperationId = "LoginUser",
            Tags = new[] { "User" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "User autheticated", typeof(AuthenticatedUser))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid user credentials")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User was not found with supplied credentials")]
        [Produces(MediaTypeNames.Application.Json)]
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(
            [FromBody, SwaggerParameter("User authentication request parameters", Required = true)]UserAuthenticationParameters authenticationParameters)
        {
            IActionResult result;
            try
            {
                result = Ok(await _loginService.Authenticate(authenticationParameters.Email, authenticationParameters.Password));
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