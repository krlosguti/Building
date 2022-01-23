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

        /// <summary>
        /// FluentValidation is used to validate information about new user
        /// </summary>
        public class UserAddValidation : AbstractValidator<UserAdd>
        {
            /// <summary>
            /// validate all the fields are not empty
            /// </summary>
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
                //context to connect to the user database
                _context = context;
                //usermanager to management the information about users and add the new user
                _userManager = userManager;
                //map user to userDTO
                _mapper = mapper;
                //generates token
                _jwtGenerator = jwtGenerator;
            }

            public async Task<UserDTO> Handle(UserAdd request, CancellationToken cancellationToken)
            {
                //search user using the email
                var existUser = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (existUser)
                {
                    throw new Exception("Email exist in the database");
                }
                //create an instance of user
                var newUser = new User
                {
                    UserName = request.UserName,
                    Email = request.Email
                };
                //create the new user 
                var result = await _userManager.CreateAsync(newUser, request.Password);
                if (!result.Succeeded)
                {
                    throw new Exception("User cann´t be inserted");
                }
                //map user to userDTO
                var userDTO = _mapper.Map<User,UserDTO>(newUser);
                //Generates token
                userDTO.Token = _jwtGenerator.CreateToken(newUser);
                return userDTO;
            }
        }
    }
}
