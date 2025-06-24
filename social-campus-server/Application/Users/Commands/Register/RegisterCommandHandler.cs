using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IEmailService emailService) : ICommandHandler<RegisterCommand>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var isEmailUnique = await userRepository.IsEmailUniqueAsync(request.Email);
        if (!isEmailUnique)
            return Result.Failure(new Error(
                "User.NotUniqueEmail",
                $"User with email {request.Email} has already exist"));

        var isLoginUnique = await userRepository.IsLoginUniqueAsync(request.Login);
        if (!isLoginUnique)
            return Result.Failure(new Error(
                "User.NotUniqueLogin",
                $"User with login {request.Login} has already exist"));

        var passwordHash = await passwordHasher.HashAsync(request.Password);

        await userRepository.AddAsync(request.Login, passwordHash, request.Email, request.FirstName, request.LastName);

        await emailService.SendEmailAsync(request.Email, "Email verification for Social-Campus",
            "To verify your email address click here");

        return Result.Success();
    }
}