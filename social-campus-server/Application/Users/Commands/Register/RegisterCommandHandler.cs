using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IHasher hasher,
    IEmailVerificationTokenRepository emailVerificationTokenRepository) : ICommandHandler<RegisterCommand>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
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

        var isTokenHashValid =
            await hasher.VerifyAsync(request.VerifyEmailToken.ToString(), verifyEmailToken.TokenHash);
        if (!isTokenHashValid)
            return Result.Failure(new Error(
                "VerifyToken.Invalid",
                "The provided verify token is invalid"));

        emailVerificationTokenRepository.Remove(verifyEmailToken);

        var isLoginUnique = await userRepository.IsLoginUniqueAsync(request.Login);
        if (!isLoginUnique)
            return Result.Failure(new Error(
                "User.NotUniqueLogin",
                $"User with login {request.Login} has already exist"));

        var passwordHash = await hasher.HashAsync(request.Password);

        await userRepository.AddAsync(request.Login, passwordHash, request.Email, request.FirstName, request.LastName);

        return Result.Success();
    }
}