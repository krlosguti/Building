using Building.IdentityServer.Core.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Building.IdentityServer.Core.JWTLogic
{
    public class JwtGenerator : IJwtGenerator
    {
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("username", user.UserName),
                new Claim("email",user.Email),
                new Claim("date",DateTime.Now.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ecBTs2BzoDRC6ct5yMPPSFMrU1xMzOLd"));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credential
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
