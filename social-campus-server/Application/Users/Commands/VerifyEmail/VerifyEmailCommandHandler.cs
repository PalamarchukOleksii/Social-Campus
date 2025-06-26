using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.VerifyEmail;

public class VerifyEmailCommandHandler(
    IEmailVerificationTokenRepository verificationTokenRepository,
    IUserRepository userRepository)
    : ICommandHandler<VerifyEmailCommand>
{
    public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var emailVerificationToken = await verificationTokenRepository.GetAsync(request.EmailVerificationTokenId);
        if (emailVerificationToken is null)
            return Result.Failure(new Error(
                "EmailVerificationToken.NotFound",
                $"EmailVerificationToken with id {request.EmailVerificationTokenId.Value} was not found"));

        if (emailVerificationToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            verificationTokenRepository.Remove(emailVerificationToken);
            return Result.Failure(new Error(
                "EmailVerificationToken.Expired",
                "The email verification token has expired"));
        }

        var user = await userRepository.GetByIdAsync(emailVerificationToken.UserId);
        if (user is null)
            return Result.Failure(new Error(
                "User.NotFound",
                $"User with id {emailVerificationToken.UserId.Value} was not found"));

        if (user.IsEmailVerified)
            return Result.Failure(new Error(
                "User.AlreadyVerified",
                $"User with id {emailVerificationToken.UserId.Value} has already verified their email"));

        userRepository.MakeUserEmailVarified(user);
        verificationTokenRepository.Remove(emailVerificationToken);

        return Result.Success();
    }
}