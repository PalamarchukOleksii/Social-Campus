using Application.Abstractions.Data;
using Application.Abstractions.Security;
using Domain.Abstractions.Repositories;
using Domain.Models.RefreshTokenModel;
using Domain.Models.TokensModel;
using Domain.Models.UserModel;
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
                    AccessToken: null,
                    RefreshToken: null,
                    ErrorMessage: "User with that email does not exist."
                );
            }

            bool isPasswordValid = passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return new LoginCommandResponse(
                    IsSuccess: false,
                    AccessToken: null,
                    RefreshToken: null,
                    ErrorMessage: "Incorrect password."
                );
            }

            Tokens tokens = jwtProvider.GenerateTokens(user);
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

            return new LoginCommandResponse(
                IsSuccess: true,
                AccessToken: tokens.AccessToken,
                RefreshToken: tokens.RefreshToken,
                ErrorMessage: null
            );
        }
    }
}