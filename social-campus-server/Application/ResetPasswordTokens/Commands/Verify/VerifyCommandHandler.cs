using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.ResetPasswordTokens.Commands.Verify;

public class VerifyCommandHandler(
    IResetPasswordTokenRepository resetPasswordTokenRepository,
    IUserRepository userRepository,
    IHasher hasher) : ICommandHandler<VerifyCommand>
{
    public async Task<Result> Handle(VerifyCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure(new Error(
                "User.NotFound",
                $"User with id {request.UserId.Value} was not found"));

        var resetPasswordToken = await resetPasswordTokenRepository.GetByUserIdAsync(user.Id);
        if (resetPasswordToken is null)
            return Result.Failure(new Error(
                "ResetToken.NotFound",
                $"Reset password token was not found for the user with userId {user.Id.Value}"));

        if (resetPasswordToken.ExpiresOnUtc < DateTime.UtcNow)
            return Result.Failure(new Error(
                "ResetToken.Invalid",
                "Reset password token is expired"));

        var isTokenHashValid = await hasher.VerifyAsync(request.Token.ToString(), resetPasswordToken.TokenHash);
        if (!isTokenHashValid)
            return Result.Failure(new Error(
                "ResetToken.Invalid",
                "The provided reset token is invalid"));

        return Result.Success();
    }
}