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
        IRefreshTokenRepository tokenRepository) : ICommandHandler<RefreshCommand, UserOnLoginRefreshDto>
    {
        public async Task<Result<UserOnLoginRefreshDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal principal;
            try
            {
                principal = await jwtProvider.GetPrincipalFromExpiredTokenAsync(request.AccessToken);
            }
            catch (Exception ex)
            {
                return Result.Failure<UserOnLoginRefreshDto>(new Error("AccessToken.InvalidToken", ex.Message));
            }

            string? email = principal.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (email == null)
            {
                return Result.Failure<UserOnLoginRefreshDto>(new Error(
                    "AccessToken.InvalidToken",
                    "The access token does not contain a valid email"));
            }

            User? user = await userRepository.GetByEmailAsync(email);
            if (user == null || user.RefreshTokenId == null)
            {
                return Result.Failure<UserOnLoginRefreshDto>(new Error(
                    "InvalidCredentials",
                    "Invalid credentials provided"));
            }

            RefreshToken? refreshToken = await tokenRepository.GetByIdAsync(user.RefreshTokenId);
            if (refreshToken == null)
            {
                return Result.Failure<UserOnLoginRefreshDto>(new Error(
                    "RefreshToken.NotFound",
                    "Refresh token not found for the user"));
            }
            else if (refreshToken.Token != request.RefreshToken)
            {
                return Result.Failure<UserOnLoginRefreshDto>(new Error(
                    "RefreshToken.InvalidToken",
                    "The provided refresh token does not match the stored refresh token"));
            }
            else if (!refreshToken.IsValid())
            {
                return Result.Failure<UserOnLoginRefreshDto>(new Error(
                    "RefreshToken.Expired",
                    "The provided refresh token has expired"));
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
