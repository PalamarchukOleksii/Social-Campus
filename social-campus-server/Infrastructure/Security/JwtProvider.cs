using Domain.Models.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Security
{
    public class JwtProvider(IOptions<JwtOptions> options)
    {
        public string CreateToken(User user)
        {
            string secretKey = options.Value.SecretKey;
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(options.Value.ExpirationInMinutes),
                SigningCredentials = credentials,
                Issuer = options.Value.Issuer,
                Audience = options.Value.Audience
            };

            JsonWebTokenHandler handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}