using BaseTemplate.Business.Abstractions.Token;
using BaseTemplate.Domain.Entities;
using BaseTemplate.Shared.Dtos.TokenDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenDto CreateAccessToken(User user)
        {
            TokenDto token = new();
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            token.Expiration = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Token:LifeTime"]));
            var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Name),
                    new(ClaimTypes.Surname, user.Surname),
                    new(ClaimTypes.Email, user?.Email),
                    new("UserId",user.Id.ToString())

                };

            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: claims
                );

            JwtSecurityTokenHandler securityTokenHandler = new();
            token.AccessToken = securityTokenHandler.WriteToken(securityToken);

            token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public string CreateRefreshToken()
        {
            TokenDto token = new TokenDto();
            byte[] number = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
