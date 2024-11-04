using Application.Data;
using Application.Security;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using MediatR;

namespace Application.Users.Commands.LoginUserCommand
{
    public class LoginUserCommandHandler(
        IJwtProvider jwtProvider,
        IUserRepository userRepository,
        IRefreshTokenRepository tokenRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork) : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return new LoginUserCommandResponse(
                    IsSuccess: false,
                    AccessToken: default,
                    RefreshToken: default,
                    ErrorMessage: "User with that email does not exist."
                );
            }

            bool isPasswordValid = passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return new LoginUserCommandResponse(
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

            return new LoginUserCommandResponse
            (
                IsSuccess: true,
                AccessToken: tokens.AccessToken,
                RefreshToken: tokens.RefreshToken,
                ErrorMessage: default
            );
        }
    }
}