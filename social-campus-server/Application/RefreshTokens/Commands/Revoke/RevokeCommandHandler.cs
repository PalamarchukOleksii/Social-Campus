using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Models.RefreshTokenModel;
using Domain.Models.TokensModel;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.RefreshTokens.Commands.Revoke
{
    public class RevokeCommandHandler(
        IRefreshTokenRepository tokenRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<RevokeCommand>
    {
        public async Task<Result> Handle(RevokeCommand request, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = await tokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
            if (refreshToken == null || refreshToken.TokenExpiryTime <= DateTime.Now)
            {
                return Result.Failure<Tokens>(new Error(
                    "RefreshToke.InvalidToken",
                    "Invalid refresh token"));
            }

            User? user = await userRepository.GetByRefreshTokenIdAsync(refreshToken.Id);
            if (user == null)
            {
                return Result.Failure<Tokens>(new Error(
                    "User.NotFound",
                    "User for the corresponding access token was not found"));
            }

            await tokenRepository.DeleteByIdAsync(refreshToken.Id);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
