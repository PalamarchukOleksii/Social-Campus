using Application.Security;
using Domain.Entities;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security
{
    public class JwtProvider(IConfiguration configuration) : IJwtProvider
    {
        public TokensModel GenerateTokens(User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            return new TokensModel
            {
                AccessToken = accessToken,
                AccessTokenExpirationInMinutes = configuration.GetValue<int>("Jwt:AccessTokenExpirationInMinutes"),
                RefreshToken = refreshToken,
                RefreshTokenExpirationInDays = configuration.GetValue<int>("Jwt:RefreshTokenExpirationInDays")
            };
        }
        public async Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token)
        {
            string secretKey = configuration["Jwt:SecretKey"]!;
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = securityKey,
                ClockSkew = TimeSpan.Zero
            };

            JsonWebTokenHandler handler = new JsonWebTokenHandler();
            TokenValidationResult result = await handler.ValidateTokenAsync(token, tokenValidationParameters);

            if (!result.IsValid)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return result.ClaimsIdentity != null ? new ClaimsPrincipal(result.ClaimsIdentity) : throw new SecurityTokenException("Invalid token");
        }

        private string GenerateAccessToken(User user)
        {
            string secretKey = configuration["Jwt:SecretKey"]!;
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.Login),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:AccessTokenExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            JsonWebTokenHandler handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[128];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}