using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.RefreshTokens.Commands.Revoke;

public class RevokeCommandHandler(
    IRefreshTokenRepository tokenRepository,
    IUserRepository userRepository) : ICommandHandler<RevokeCommand>
{
    public async Task<Result> Handle(RevokeCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await tokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
        if (refreshToken == null || refreshToken.TokenExpiryTime <= DateTime.Now)
            return Result.Failure<TokensDto>(new Error(
                "RefreshToke.InvalidToken",
                "Invalid refresh token"));

        var user = await userRepository.GetByRefreshTokenIdAsync(refreshToken.Id);
        if (user == null)
            return Result.Failure<TokensDto>(new Error(
                "User.NotFound",
                "User for the corresponding access token was not found"));

        await tokenRepository.DeleteByIdAsync(refreshToken.Id);

        return Result.Success();
    }
}