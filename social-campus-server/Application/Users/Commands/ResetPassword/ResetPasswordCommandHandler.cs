using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    IResetPasswordTokenRepository resetPasswordTokenRepository,
    IHasher hasher) : ICommandHandler<ResetPasswordCommand>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
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

        var isTokenHashValid =
            await hasher.VerifyAsync(request.ResetPasswordToken.ToString(), resetPasswordToken.TokenHash);
        if (!isTokenHashValid)
            return Result.Failure(new Error(
                "ResetToken.Invalid",
                "The provided reset token is invalid"));

        resetPasswordTokenRepository.Remove(resetPasswordToken);

        var passwordHash = await hasher.HashAsync(request.NewPassword);
        if (passwordHash is null)
            return Result.Failure(new Error(
                "Hasher.Failed",
                "Unable to generate secure password hash"));

        userRepository.UpdatePassword(user, passwordHash);

        return Result.Success();
    }
}