﻿using Application.Abstractions.Data;
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
        IRefreshTokenRepository tokenRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<RefreshCommand, TokensDto>
    {
        public async Task<Result<TokensDto>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal principal = await jwtProvider.GetPrincipalFromExpiredTokenAsync(request.AccessToken);
            string? email = principal.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (email == null)
            {
                return Result.Failure<TokensDto>(new Error(
                    "AccessToke.InvalidToken",
                    "Invalid access token"));
            }

            User? user = await userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return Result.Failure<TokensDto>(new Error(
                    "User.NotFound",
                    "User for the corresponding access token was not found"));
            }

            if (user.RefreshTokenId == null)
            {
                return Result.Failure<TokensDto>(new Error(
                    "RefreshToken.NotFound",
                    "Refresh token for user was not found"));
            }

            RefreshToken? refreshToken = await tokenRepository.GetByIdAsync(user.RefreshTokenId);
            if (refreshToken == null || refreshToken.Token != request.RefreshToken || refreshToken.TokenExpiryTime <= DateTime.Now)
            {
                return Result.Failure<TokensDto>(new Error(
                    "RefreshToke.InvalidToken",
                    "Invalid refresh token"));
            }

            TokensDto tokens = jwtProvider.GenerateTokens(user);
            tokenRepository.Update(refreshToken, tokens.RefreshToken, tokens.RefreshTokenExpirationInDays);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(tokens);
        }
    }
}
