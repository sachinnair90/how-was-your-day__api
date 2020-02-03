using Infrastructure;
using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using BusinessLogic.Exceptions;
using DataAccess.Exceptions;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IOptions<Configuration> options;

        public AuthenticateService(IUnitOfWork unitOfWork, IMapper mapper, ITokenGenerator tokenGenerator, IOptions<Configuration> options)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.tokenGenerator = tokenGenerator;
            this.options = options;
        }

        public async Task<AuthenticatedUser> Authenticate(string email, string password)
        {
            User user;

            try
            {
                user = await unitOfWork.UserRepository.GetUserFromCredentials(email, password);
            }
            catch (InvalidUserPasswordException ex)
            {
                throw new InvalidCredentialsException("Invalid password was supplied", ex);
            }
            catch (DataAccess.Exceptions.UserNotFoundException ex)
            {
                throw new Exceptions.UserNotFoundException($"User with the {email} is not found", ex);
            }

            var authenticatedUser = mapper.Map<User, AuthenticatedUser>(user);

            authenticatedUser.Token = tokenGenerator.GetToken(new Claim[]
            {
                new Claim(ClaimTypes.Name, authenticatedUser.FirstName),
                new Claim(ClaimTypes.Surname, authenticatedUser.LastName),
                new Claim(ClaimTypes.Email, authenticatedUser.Email),
                new Claim(ClaimTypes.NameIdentifier, authenticatedUser.Id.ToString())
            }, options.Value.TokenExpiryDays, options.Value.Security.JwtSecret);

            return authenticatedUser;
        }
    }
}