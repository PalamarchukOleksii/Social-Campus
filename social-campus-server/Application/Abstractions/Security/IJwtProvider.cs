using System.Security.Claims;
using Application.Dtos;
using Domain.Models.UserModel;

namespace Application.Abstractions.Security;

public interface IJwtProvider
{
    public TokensDto GenerateTokens(User user);
    public Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
}