﻿using Application.Abstractions.Messaging;
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
        IPasswordHasher passwordHasher) : ICommandHandler<LoginCommand, TokensDto>
    {
        public async Task<Result<TokensDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return Result.Failure<TokensDto>(new Error(
                    "User.NotFound",
                    $"User with email {request.Email} was not found"));
            }

            bool isPasswordValid = await passwordHasher.VerifyAsync(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Result.Failure<TokensDto>(new Error(
                    "User.IncorrectPassword",
                    "Incorrect password"));
            }

            TokensDto tokens = jwtProvider.GenerateTokens(user);
            if (user.RefreshTokenId.Value != Guid.Empty)
            {
                RefreshToken? existingRefreshToken = await tokenRepository.GetByIdAsync(user.RefreshTokenId);
                if (existingRefreshToken != null && existingRefreshToken.IsValid())
                {
                    tokens.RefreshTokenExpirationInDays = existingRefreshToken.DaysUntilExpiration();
                    tokens.RefreshToken = existingRefreshToken.Token;
                }
                else if (existingRefreshToken != null)
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

            return Result.Success(tokens);
        }
    }
}