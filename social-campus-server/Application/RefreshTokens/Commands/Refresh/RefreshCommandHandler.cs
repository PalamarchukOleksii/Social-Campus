using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Domain.Abstractions.Repositories;
using Domain.Models.RefreshTokenModel;
using Domain.Models.TokensModel;
using Domain.Models.UserModel;
using Domain.Shared;
using System.Security.Claims;

namespace Application.RefreshTokens.Commands.Refresh
{
    public class RefreshCommandHandler(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IRefreshTokenRepository tokenRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<RefreshCommand, Tokens>
    {
        public async Task<Result<Tokens>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal principal = await jwtProvider.GetPrincipalFromExpiredTokenAsync(request.AccessToken);
            string? email = principal.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (email == null)
            {
                return Result.Failure<Tokens>(new Error(
                    "AccessToke.InvalidToken",
                    "Invalid access token"));
            }

            User? user = await userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return Result.Failure<Tokens>(new Error(
                    "User.NotFound",
                    "User for the corresponding access token was not found"));
            }

            if (user.RefreshTokenId == null)
            {
                return Result.Failure<Tokens>(new Error(
                    "RefreshToken.NotFound",
                    "Refresh token for user was not found"));
            }

            RefreshToken? refreshToken = await tokenRepository.GetByIdAsync(user.RefreshTokenId);
            if (refreshToken == null || refreshToken.Token != request.RefreshToken || refreshToken.TokenExpiryTime <= DateTime.Now)
            {
                return Result.Failure<Tokens>(new Error(
                    "RefreshToke.InvalidToken",
                    "Invalid refresh token"));
            }

            Tokens tokens = jwtProvider.GenerateTokens(user);
            tokenRepository.Update(refreshToken, tokens.RefreshToken, tokens.RefreshTokenExpirationInDays);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(tokens);
        }
    }
}
