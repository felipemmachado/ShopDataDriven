using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shop.Models;

namespace Shop.Services {

    public static class TokenService 
    {
        public static string GenerateToken(User user)
        {
            // lib que gera o token
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // pega o array de bytes do token
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            // o que vai ter no token e configs
            var tokenDescriptor = new SecurityTokenDescriptor {

                // o que vai conter no token
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                }),

                // tempo de expiração
                Expires = DateTime.UtcNow.AddHours(10),
                
                // o tipo de criptografia do token
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            // gera o token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);            
        }
    }
}