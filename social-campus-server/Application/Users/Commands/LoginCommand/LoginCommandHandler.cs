using Application.Data;
using Application.Security;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Users.Commands.LoginCommand
{
    public class LoginCommandHandler(
        IJwtProvider jwtProvider,
        IUserRepository userRepository,
        IRefreshTokenRepository tokenRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork) : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return new LoginCommandResponse(
                    IsSuccess: false,
                    AccessToken: default,
                    RefreshToken: default,
                    ErrorMessage: "User with that email does not exist."
                );
            }

            bool isPasswordValid = passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return new LoginCommandResponse(
                    IsSuccess: false,
                    AccessToken: default,
                    RefreshToken: default,
                    ErrorMessage: "Incorrect password."
                );
            }

            TokensModel tokens = jwtProvider.GenerateTokens(user);
            RefreshToken? existingRefreshToken = await tokenRepository.GetByIdAsync(user.RefreshTokenId);
            if (existingRefreshToken != null)
            {
                tokenRepository.UpdateAsync(existingRefreshToken, tokens.RefreshToken, tokens.RefreshTokenExpirationInDays);
            }
            else
            {
                RefreshToken refreshToken = await tokenRepository.AddAsync(tokens.RefreshToken, tokens.RefreshTokenExpirationInDays);
                user.RefreshTokenId = refreshToken.Id;
            }

            await unitOfWork.SaveChangesAsync();

            return new LoginCommandResponse
            (
                IsSuccess: true,
                AccessToken: tokens.AccessToken,
                RefreshToken: tokens.RefreshToken,
                ErrorMessage: default
            );
        }
    }
}