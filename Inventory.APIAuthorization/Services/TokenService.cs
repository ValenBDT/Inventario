using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Inventory.APIAuthorization.Services.Interfaces;
using Inventory.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.APIAuthorization.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _ssKey;


        public TokenService(IConfiguration configuration)
        {
            _ssKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["tokenKey"]));
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.NameId,user.Email ));
            claims.Add(new Claim("Id", user.Id.ToString()));
            claims.Add(new Claim("Name", user.Name));

            var credentials = new SigningCredentials(_ssKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Issuer = "issuer",
                Audience = "issuer",
                Subject = new (claims),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}