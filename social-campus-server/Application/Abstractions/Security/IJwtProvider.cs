using Domain.Entities;
using Domain.Models;
using System.Security.Claims;

namespace Application.Abstractions.Security
{
    public interface IJwtProvider
    {
        public TokensModel GenerateTokens(User user);
        public Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
    }
}
