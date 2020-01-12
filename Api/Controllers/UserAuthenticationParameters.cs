using System.ComponentModel.DataAnnotations;

namespace Api.Controllers
{
    public class UserAuthenticationParameters
    {
        [Required, EmailAddress]
        public string Email { get; internal set; }

        [Required, MinLength(8)]
        public string Password { get; internal set; }
    }
}