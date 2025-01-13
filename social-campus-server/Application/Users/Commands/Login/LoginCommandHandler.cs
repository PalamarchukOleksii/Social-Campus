using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Users.Commands.Login
{
    public class LoginCommandHandler(
        IJwtProvider jwtProvider,
        IUserRepository userRepository,
        IRefreshTokenRepository tokenRepository,
        IPasswordHasher passwordHasher) : ICommandHandler<LoginCommand, UserOnLoginDto>
    {
        public async Task<Result<UserOnLoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return Result.Failure<UserOnLoginDto>(new Error(
                    "User.NotFound",
                    $"User with email {request.Email} was not found"));
            }

            bool isPasswordValid = await passwordHasher.VerifyAsync(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Result.Failure<UserOnLoginDto>(new Error(
                    "User.IncorrectPassword",
                    "Incorrect password"));
            }

            TokensDto tokens = jwtProvider.GenerateTokens(user);
            RefreshToken? existingRefreshToken = user.RefreshTokenId.Value != Guid.Empty
                ? await tokenRepository.GetByIdAsync(user.RefreshTokenId)
                : null;

            if (existingRefreshToken != null)
            {
                tokenRepository.Update(existingRefreshToken, tokens.RefreshToken, tokens.RefreshTokenExpirationInSeconds);
            }
            else
            {
                RefreshToken refreshToken = await tokenRepository.AddAsync(tokens.RefreshToken, tokens.RefreshTokenExpirationInSeconds, user.Id);
                user.SetRefreshTokenId(refreshToken.Id);
            }

            return Result.Success(new UserOnLoginDto
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