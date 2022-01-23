using AutoMapper;
using Building.IdentityServer.Core.Context;
using Building.IdentityServer.Core.DTO;
using Building.IdentityServer.Core.Entities;
using Building.IdentityServer.Core.JWTLogic;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Building.IdentityServer.Core.Application
{
    public class Login
    {
        /// <summary>
        /// model to log in the user
        /// the log in needs username and passsword
        /// </summary>
        public class UserLogin : IRequest<UserDTO>
        {
            public string UserName { get; set; }    
            public string Password { get; set; }
        }

        /// <summary>
        /// validates the username and password are not empty
        /// </summary>
        public class UserLoginValidation: AbstractValidator<UserLogin>
        {
            public UserLoginValidation()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class UserLoginHandler : IRequestHandler<UserLogin, UserDTO>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly SignInManager<User> _signInManager;

            public UserLoginHandler(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper, IJwtGenerator jwtGenerator, SignInManager<User> signInManager)
            {
                //connect to the user database
                _context = context;
                //management the user information
                _userManager = userManager;
                //map user to user DTO
                _mapper = mapper;
                //generator of a token
                _jwtGenerator = jwtGenerator;
                //allow check the password is right
                _signInManager = signInManager;
            }
            public async Task<UserDTO> Handle(UserLogin request, CancellationToken cancellationToken)
            {
                //search user using username parameter
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    throw new Exception("User doesn't exist");
                }
                //check the password is right
                var result = await _signInManager.CheckPasswordSignInAsync(user,request.Password,false);
                if (!result.Succeeded)
                {
                    throw new Exception("Invalid password");
                }
                //map user to userDTO
                var userDTO = _mapper.Map<User, UserDTO>(user);
                //generates token
                userDTO.Token = _jwtGenerator.CreateToken(user);
                return userDTO;
            }
        }
    }
}
