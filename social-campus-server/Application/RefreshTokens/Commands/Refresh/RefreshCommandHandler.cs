using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.RefreshTokens.Commands.Refresh
{
    public class RefreshCommandHandler(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IRefreshTokenRepository tokenRepository) : ICommandHandler<RefreshCommand, UserOnLoginRefreshDto>
    {
        public async Task<Result<UserOnLoginRefreshDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await tokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
            if (refreshToken == null)
            {
                return Result.Failure<UserOnLoginRefreshDto>(new Error(
                    "RefreshToken.NotFound",
                    "Refresh token not found for the user"));
            }
            else if (!refreshToken.IsValid())
            {
                return Result.Failure<UserOnLoginRefreshDto>(new Error(
                    "RefreshToken.Expired",
                    "The provided refresh token has expired"));
            }

            User? user = await userRepository.GetByRefreshTokenIdAsync(refreshToken.Id);
            if (user == null || user.RefreshTokenId == null)
            {
                return Result.Failure<UserOnLoginRefreshDto>(new Error(
                    "RefreshToken.Invalid",
                    "No user exists with the provided refresh token"));
            }

            TokensDto tokens = jwtProvider.GenerateTokens(user);
            tokenRepository.Update(refreshToken, tokens.RefreshToken, tokens.RefreshTokenExpirationInSeconds);

            return Result.Success(new UserOnLoginRefreshDto
            {
                Tokens = tokens,
                ShortUser = new ShortUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Login = user.Login,
                    ProfileImageData = user.ProfileImageData
                }
            });
        }
    }
}
