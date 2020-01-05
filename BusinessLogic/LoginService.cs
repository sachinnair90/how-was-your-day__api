using Infrastructure;
using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BusinessLogic
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IOptions<Configuration> options;

        public LoginService(IUnitOfWork unitOfWork, IMapper mapper, ITokenGenerator tokenGenerator, IOptions<Configuration> options)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.tokenGenerator = tokenGenerator;
            this.options = options;
        }
        public AuthenticatedUser Authenticate(string email, string password)
        {
            var user = unitOfWork.UserRepository.GetUserFromCredentials(email, password);

            var authenticatedUser = mapper.Map<User, AuthenticatedUser>(user);

            authenticatedUser.Token = tokenGenerator.GetToken(new Claim[]
            {
                new Claim(ClaimTypes.Name, authenticatedUser.FirstName),
                new Claim(ClaimTypes.Surname, authenticatedUser.LastName),
                new Claim(ClaimTypes.Email, authenticatedUser.Email),
                new Claim(ClaimTypes.NameIdentifier, authenticatedUser.Id.ToString())

            }, options.Value.MaxPasswordExpiryDays, options.Value.Secret);

            return authenticatedUser;
        }
    }
}
