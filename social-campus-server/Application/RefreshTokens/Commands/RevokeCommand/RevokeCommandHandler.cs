using Application.Abstractions.Data;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.RefreshTokens.Commands.RevokeCommand
{
    public class RevokeCommandHandler(
        IRefreshTokenRepository tokenRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RevokeCommandRequest, RevokeCommandResponse>
    {
        public async Task<RevokeCommandResponse> Handle(RevokeCommandRequest request, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await tokenRepository.GetByTokenAsync(request.RefreshToken);
            if (refreshToken == null || refreshToken.TokenExpiryTime <= DateTime.Now)
            {
                return new RevokeCommandResponse(
                    IsSuccess: false,
                    ErrorMessage: "Invalid refresh token.");
            }

            User? user = await userRepository.GetByRefreshTokenIdAsync(refreshToken.Id);
            if (user == null)
            {
                return new RevokeCommandResponse(
                    IsSuccess: false,
                    ErrorMessage: "Invalid refresh token.");
            }

            tokenRepository.DeleteByIdAsync(refreshToken.Id);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new RevokeCommandResponse(
                IsSuccess: true,
                ErrorMessage: default);
        }
    }
}
