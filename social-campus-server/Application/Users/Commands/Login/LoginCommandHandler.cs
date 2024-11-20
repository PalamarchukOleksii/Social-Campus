using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Domain.Abstractions.Repositories;
using Domain.Dtos;
using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Users.Commands.Login
{
    public class LoginCommandHandler(
        IJwtProvider jwtProvider,
        IUserRepository userRepository,
        IRefreshTokenRepository tokenRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork) : ICommandHandler<LoginCommand, TokensDto>
    {
        public async Task<Result<TokensDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return Result.Failure<TokensDto>(new Error(
                    "User.NotFound",
                    $"User with that email {request.Email} was not found"));
            }

            bool isPasswordValid = passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Result.Failure<TokensDto>(new Error(
                    "User.IncorrectPassword",
                    "Incorrect password"));
            }

            TokensDto tokens = jwtProvider.GenerateTokens(user);
            if (user.RefreshTokenId != null)
            {
                RefreshToken? existingRefreshToken = await tokenRepository.GetByIdAsync(user.RefreshTokenId);
                if (existingRefreshToken != null)
                {
                    tokenRepository.Update(existingRefreshToken, tokens.RefreshToken, tokens.RefreshTokenExpirationInDays);
                }
                else
                {
                    RefreshToken refreshToken = await tokenRepository.AddAsync(tokens.RefreshToken, tokens.RefreshTokenExpirationInDays, user.Id);
                    user.SetRefreshTokenId(refreshToken.Id);
                }
            }
            else
            {
                RefreshToken refreshToken = await tokenRepository.AddAsync(tokens.RefreshToken, tokens.RefreshTokenExpirationInDays, user.Id);
                user.SetRefreshTokenId(refreshToken.Id);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(tokens);
        }
    }
}