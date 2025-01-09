using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;
using Domain.Shared;
using System.Security.Claims;

namespace Application.RefreshTokens.Commands.Refresh
{
    public class RefreshCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider,
    IRefreshTokenRepository tokenRepository) : ICommandHandler<RefreshCommand, TokensDto>
    {
        public async Task<Result<TokensDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal principal = await jwtProvider.GetPrincipalFromExpiredTokenAsync(request.AccessToken);
            string? email = principal.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (email == null)
            {
                return Result.Failure<TokensDto>(new Error(
                    "AccessToken.InvalidToken",
                    "Invalid access token"));
            }

            User? user = await userRepository.GetByEmailAsync(email);
            if (user == null || user.RefreshTokenId == null)
            {
                return Result.Failure<TokensDto>(new Error(
                    "InvalidCredentials",
                    "Invalid credentials provided"));
            }

            RefreshToken? refreshToken = await tokenRepository.GetByIdAsync(user.RefreshTokenId);
            if (refreshToken == null ||
                refreshToken.Token != request.RefreshToken ||
                !refreshToken.IsValid())
            {
                return Result.Failure<TokensDto>(new Error(
                    "RefreshToken.InvalidToken",
                    "Invalid or expired refresh token"));
            }

            TokensDto tokens = jwtProvider.GenerateTokens(user);
            tokenRepository.Update(refreshToken, tokens.RefreshToken, tokens.RefreshTokenExpirationInDays);

            return Result.Success(new TokensDto
            {
                AccessToken = tokens.AccessToken,
                AccessTokenExpirationInMinutes = tokens.AccessTokenExpirationInMinutes,
                RefreshToken = tokens.RefreshToken,
                RefreshTokenExpirationInDays = tokens.RefreshTokenExpirationInDays
            });
        }
    }
}
