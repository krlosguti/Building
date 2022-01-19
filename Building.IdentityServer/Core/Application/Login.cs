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
        public class UserLogin : IRequest<UserDTO>
        {
            public string UserName { get; set; }    
            public string Password { get; set; }
        }

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
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
                _jwtGenerator = jwtGenerator;
                _signInManager = signInManager;
            }
            public async Task<UserDTO> Handle(UserLogin request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    throw new Exception("User doesn't exist");
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user,request.Password,false);
                if (!result.Succeeded)
                {
                    throw new Exception("Invalid password");
                }
                var userDTO = _mapper.Map<User, UserDTO>(user);
                userDTO.Token = _jwtGenerator.CreateToken(user);
                return userDTO;
            }
        }
    }
}
