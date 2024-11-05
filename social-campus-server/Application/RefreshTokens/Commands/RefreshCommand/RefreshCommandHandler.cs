using Application.Data;
using Application.Security;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using System.Security.Claims;

namespace Application.RefreshTokens.Commands.RefreshCommand
{
    public class RefreshCommandHandler(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IRefreshTokenRepository tokenRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RefreshCommandRequest, RefreshCommandResponse>
    {
        public async Task<RefreshCommandResponse> Handle(RefreshCommandRequest request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal principal = await jwtProvider.GetPrincipalFromExpiredTokenAsync(request.AccessToken);
            string? email = principal.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (email == null)
            {
                return new RefreshCommandResponse(
                    IsSuccess: false,
                    AccessToken: default,
                    RefreshToken: default,
                    ErrorMessage: "Invalid access token.");
            }

            User? user = await userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return new RefreshCommandResponse(
                    IsSuccess: false,
                    AccessToken: default,
                    RefreshToken: default,
                    ErrorMessage: "Invalid access token.");
            }

            RefreshToken? refreshToken = await tokenRepository.GetByIdAsync(user.RefreshTokenId);
            if (refreshToken == null || refreshToken.Token != request.RefreshToken || refreshToken.TokenExpiryTime <= DateTime.Now)
            {
                return new RefreshCommandResponse(
                    IsSuccess: false, AccessToken: default,
                    RefreshToken: default,
                    ErrorMessage: "Invalid refresh token.");
            }

            TokensModel tokens = jwtProvider.GenerateTokens(user);
            tokenRepository.UpdateAsync(refreshToken, tokens.RefreshToken, tokens.RefreshTokenExpirationInDays);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new RefreshCommandResponse(
                IsSuccess: true,
                AccessToken: tokens.AccessToken,
                RefreshToken: tokens.RefreshToken,
                ErrorMessage: default);
        }
    }
}
