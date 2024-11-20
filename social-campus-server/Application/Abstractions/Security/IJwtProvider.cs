using Domain.Dtos;
using Domain.Models.UserModel;
using System.Security.Claims;

namespace Application.Abstractions.Security
{
    public interface IJwtProvider
    {
        public TokensDto GenerateTokens(User user);
        public Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
    }
}
