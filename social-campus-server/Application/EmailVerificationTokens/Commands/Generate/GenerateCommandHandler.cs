using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Application.Helpers;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.EmailVerificationTokens.Commands.Generate;

public class GenerateCommandHandler(
    IEmailVerificationTokenRepository emailVerificationTokenRepository,
    IUserRepository userRepository,
    IEmailService emailService,
    IEmailLinkFactory emailLinkFactory,
    IHasher hasher) : ICommandHandler<GenerateCommand>
{
    public async Task<Result> Handle(GenerateCommand request, CancellationToken cancellationToken)
    {
        var isEmailUnique = await userRepository.IsEmailUniqueAsync(request.Email);
        if (!isEmailUnique)
            return Result.Failure(new Error(
                "User.NotUniqueEmail",
                $"User with email {request.Email} has already exist"));

        var token = Guid.NewGuid();
        var tokenHash = await hasher.HashAsync(token.ToString());
        await emailVerificationTokenRepository.AddAsync(request.Email, tokenHash);

        var verificationLink = emailLinkFactory.CreateEmailVerificationLink(token, request.Email);
        if (verificationLink is null)
            return Result.Failure(new Error(
                "Email.LinkGenerationFailed",
                "Unable to generate email verification link"));

        var messageBody = EmailTemplateHelpers.GetResetPasswordHtml(request.Email, verificationLink);
        await emailService.SendEmailAsync(request.Email, "Password Reset Request", messageBody, true);

        return Result.Success();
    }
}