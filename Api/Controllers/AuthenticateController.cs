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
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _authService;

        public AuthenticateController(IAuthenticateService authenticateService)
        {
            _authService = authenticateService;
        }

        [SwaggerOperation(
            Summary = "Authenticate user using credentials",
            Description = "Authenticate user using credentials",
            OperationId = "Authenticate",
            Tags = new[] { "Auth" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Autheticated", typeof(AuthenticatedUser))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid user credentials")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User was not found with supplied credentials")]
        [Produces(MediaTypeNames.Application.Json)]
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Authenticate(
            [FromBody, SwaggerParameter("Authentication request parameters", Required = true)]UserAuthenticationParameters authenticationParameters)
        {
            IActionResult result;
            try
            {
                result = Ok(await _authService.Authenticate(authenticationParameters.Email, authenticationParameters.Password));
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

        [SwaggerOperation(
            Summary = "Check if user is authenticated",
            Description = "Check if user is authenticated, returns empty response",
            OperationId = "IsAuthenticated",
            Tags = new[] { "Auth" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Autheticated")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthenticated")]
        [HttpGet, Route("IsAuthenticated")]
        public IActionResult IsAuthenticated()
        {
            return Ok();
        }
    }
}