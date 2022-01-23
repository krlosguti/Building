using Building.IdentityServer.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Building.IdentityServer.Core.JWTLogic
{
    /// <summary>
    /// Class to generates a new token
    /// </summary>
    public class JwtGenerator : IJwtGenerator
    {
        //Configuration is used to retrieve information of the private key stored in appsettings.json
        private readonly IConfiguration Configuration;

        public JwtGenerator(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string CreateToken(User user)
        {
            //creates the username, email and date claims for the new token
            var claims = new List<Claim>
            {
                new Claim("username", user.UserName),
                new Claim("email",user.Email),
                new Claim("date",DateTime.Now.ToString())
            };
            //get the private key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            //create the credential using the private key
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //create the token descriptor with one day of expiration
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credential
            };
            //create el new token using a new instance o JwtSecurityTokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
