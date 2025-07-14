using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Application.Abstractions.Storage;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.RefreshTokenModel;
using Domain.Shared;

namespace Application.RefreshTokens.Commands.Refresh;

public class RefreshCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider,
    IRefreshTokenRepository tokenRepository,
    IStorageService storageService) : ICommandHandler<RefreshCommand, UserLoginRefreshDto>
{
    public async Task<Result<UserLoginRefreshDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await tokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
        if (refreshToken == null)
            return Result.Failure<UserLoginRefreshDto>(new Error(
                "RefreshToken.NotFound",
                "Refresh token not found for the user"));
        if (!refreshToken.IsValid())
            return Result.Failure<UserLoginRefreshDto>(new Error(
                "RefreshToken.Expired",
                "The provided refresh token has expired"));

        var user = await userRepository.GetByRefreshTokenIdAsync(refreshToken.Id);
        if (user == null || user.RefreshTokenId.Value == new RefreshTokenId(Guid.Empty).Value)
            return Result.Failure<UserLoginRefreshDto>(new Error(
                "RefreshToken.Invalid",
                "No user exists with the provided refresh token"));

        var tokens = jwtProvider.GenerateTokens(user);
        tokenRepository.Update(refreshToken, tokens.RefreshToken, tokens.RefreshTokenExpirationInSeconds);

        return Result.Success(new UserLoginRefreshDto
        {
            Tokens = tokens,
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Login = user.Login,
            ProfileImageUrl = await storageService.GetPresignedUrlAsync(user.ProfileImageObjectKey)
        });
    }
}