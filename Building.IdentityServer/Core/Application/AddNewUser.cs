using AutoMapper;
using Building.IdentityServer.Core.Context;
using Building.IdentityServer.Core.DTO;
using Building.IdentityServer.Core.Entities;
using Building.IdentityServer.Core.JWTLogic;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Building.IdentityServer.Core.Application
{
    public class AddNewUser
    {
        /// <summary>
        /// To insert a new user, mediatr request receive information as UserName, Email and Password
        /// return object UserDTO
        /// </summary>
        public class UserAdd: IRequest<UserDTO>
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class UserAddValidation : AbstractValidator<UserAdd>
        {
            public UserAddValidation()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
        public class UserAddHandler : IRequestHandler<UserAdd, UserDTO>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;
            private readonly IJwtGenerator _jwtGenerator;

            public UserAddHandler(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper, IJwtGenerator jwtGenerator)
            {
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<UserDTO> Handle(UserAdd request, CancellationToken cancellationToken)
            {
                var existUser = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (existUser)
                {
                    throw new Exception("Email exist in the database");
                }
                var newUser = new User
                {
                    UserName = request.UserName,
                    Email = request.Email
                };
                var result = await _userManager.CreateAsync(newUser, request.Password);
                if (!result.Succeeded)
                {
                    throw new Exception("User cann´t be inserted");
                }
                var userDTO = _mapper.Map<User,UserDTO>(newUser);
                userDTO.Token = _jwtGenerator.CreateToken(newUser);
                return userDTO;
            }
        }
    }
}
