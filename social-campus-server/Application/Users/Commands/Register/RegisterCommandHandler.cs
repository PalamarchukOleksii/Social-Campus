using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Application.Helpers;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Users.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IEmailService emailService,
    IEmailVerificationTokenRepository emailVerificationTokenRepository,
    IEmailVerificationLinkFactory emailVerificationLinkFactory) : ICommandHandler<RegisterCommand>
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

        var registeredUser = await userRepository.AddAsync(request.Login, passwordHash, request.Email,
            request.FirstName, request.LastName);

        var emailVerificationToken = await emailVerificationTokenRepository.AddAsync(registeredUser.Id);

        var verificationLink = emailVerificationLinkFactory.Create(emailVerificationToken.Id);
        if (verificationLink is null)
            return Result.Failure(new Error(
                "Email.LinkGenerationFailed",
                "Unable to generate email verification link"));

        var messageBody = EmailTemplateHelpers.GetVerifyEmailHtml(registeredUser.FirstName, verificationLink);
        await emailService.SendEmailAsync(request.Email, "Email verification for Social-Campus", messageBody, true);

        return Result.Success();
    }
}