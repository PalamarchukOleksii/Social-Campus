using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.EmailVerificationTokens.Commands.Verify;

public class VerifyCommandHadler(
    IEmailVerificationTokenRepository emailVerificationTokenRepository,
    IUserRepository userRepository,
    IHasher hasher) : ICommandHandler<VerifyCommand>
{
    public async Task<Result> Handle(VerifyCommand request, CancellationToken cancellationToken)
    {
        var isEmailUnique = await userRepository.IsEmailUniqueAsync(request.Email);
        if (!isEmailUnique)
            return Result.Failure(new Error(
                "User.NotUniqueEmail",
                $"User with email {request.Email} has already exist"));

        var verifyEmailToken = await emailVerificationTokenRepository.GetByEmailAsync(request.Email);
        if (verifyEmailToken is null)
            return Result.Failure(new Error(
                "VerifyToken.NotFound",
                $"Verify email token was not found for the email {request.Email}"));

        if (verifyEmailToken.ExpiresOnUtc < DateTime.UtcNow)
            return Result.Failure(new Error(
                "VerifyToken.Invalid",
                "Verify email token is expired"));

        var isTokenHashValid = await hasher.VerifyAsync(request.Token.ToString(), verifyEmailToken.TokenHash);
        if (!isTokenHashValid)
            return Result.Failure(new Error(
                "VerifyToken.Invalid",
                "The provided verify token is invalid"));

        return Result.Success();
    }
}