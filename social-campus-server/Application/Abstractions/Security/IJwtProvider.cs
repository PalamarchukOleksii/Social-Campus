using Domain.Models.TokensModel;
using Domain.Models.UserModel;
using System.Security.Claims;

namespace Application.Abstractions.Security
{
    public interface IJwtProvider
    {
        public Tokens GenerateTokens(User user);
        public Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
    }
}
