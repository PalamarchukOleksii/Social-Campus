using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.Login;

public class LoginCommandHandler(
    IJwtProvider jwtProvider,
    IUserRepository userRepository,
    IRefreshTokenRepository tokenRepository,
    IPasswordHasher passwordHasher) : ICommandHandler<LoginCommand, UserLoginRefreshDto>
{
    public async Task<Result<UserLoginRefreshDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null)
            return Result.Failure<UserLoginRefreshDto>(new Error(
                "User.NotFound",
                $"User with email {request.Email} was not found"));

        var isPasswordValid = await passwordHasher.VerifyAsync(request.Password, user.PasswordHash);
        if (!isPasswordValid)
            return Result.Failure<UserLoginRefreshDto>(new Error(
                "User.IncorrectPassword",
                "Incorrect password"));

        var tokens = jwtProvider.GenerateTokens(user);
        var existingRefreshToken = user.RefreshTokenId.Value != Guid.Empty
            ? await tokenRepository.GetByIdAsync(user.RefreshTokenId)
            : null;

        if (existingRefreshToken != null)
        {
            tokenRepository.Update(existingRefreshToken, tokens.RefreshToken, tokens.RefreshTokenExpirationInSeconds);
        }
        else
        {
            var refreshToken =
                await tokenRepository.AddAsync(tokens.RefreshToken, tokens.RefreshTokenExpirationInSeconds, user.Id);
            user.SetRefreshTokenId(refreshToken.Id);
        }

        return Result.Success(new UserLoginRefreshDto
        {
            Tokens = tokens,
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Login = user.Login,
            ProfileImageData = user.ProfileImageData
        });
    }
}