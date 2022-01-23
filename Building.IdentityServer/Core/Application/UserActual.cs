using AutoMapper;
using Building.IdentityServer.Core.DTO;
using Building.IdentityServer.Core.Entities;
using Building.IdentityServer.Core.JWTLogic;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Building.IdentityServer.Core.Application
{
    public class UserActual
    {
        /// <summary>
        /// Get the current logged in user 
        /// </summary>
        public class GetUserActual : IRequest<UserDTO>{ }

        public class GetUserActualHandler : IRequestHandler<GetUserActual, UserDTO>
        {
            /// <summary>
            /// _userManager to store information of the current user
            /// _userSession to retrieve the information about current user
            /// _jwtGenerator to generate a new token claims username, email and datetime.now
            /// _mapper Map user to userDTO
            /// </summary>
            private readonly UserManager<User> _userManager;
            private readonly IUserSession _userSession;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IMapper _mapper;

            public GetUserActualHandler(UserManager<User> userManager, IUserSession userSession, IJwtGenerator jwtGenerator, IMapper mapper)
            {
                _userManager = userManager;
                _userSession = userSession;
                _jwtGenerator = jwtGenerator;
                _mapper = mapper;
            }

            public async Task<UserDTO> Handle(GetUserActual request, CancellationToken cancellationToken)
            {
                //Get the current logged in user
                var user = await _userManager.FindByNameAsync(_userSession.GetUserNameSession());
                if (user == null)
                {
                    throw new Exception("user doesn't exist");
                }
                //map the current logged in user
                var userDTO = _mapper.Map<User, UserDTO>(user);
                //generate the token
                userDTO.Token = _jwtGenerator.CreateToken(user);
                return userDTO;
            }
        }
    }
}
