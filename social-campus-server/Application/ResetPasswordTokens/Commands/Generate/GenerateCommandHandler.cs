using Application.Abstractions.Email;
using Application.Abstractions.Messaging;
using Application.Abstractions.Security;
using Application.Helpers;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.ResetPasswordTokens.Commands.Generate;

public class GenerateCommandHandler(
    IResetPasswordTokenRepository resetPasswordTokenRepository,
    IUserRepository userRepository,
    IEmailService emailService,
    IEmailLinkFactory emailLinkFactory,
    IHasher hasher) : ICommandHandler<GenerateCommand>
{
    public async Task<Result> Handle(GenerateCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null)
            return Result.Failure(new Error(
                "User.NotFound",
                $"User with email {request.Email} was not found"));

        var tokenHash = await hasher.HashAsync(Guid.NewGuid().ToString());
        var resetPasswordToken = await resetPasswordTokenRepository.AddAsync(user.Id, tokenHash);

        var verificationLink = emailLinkFactory.CreateResetPasswordLink(resetPasswordToken.Id, user.Id);
        if (verificationLink is null)
            return Result.Failure(new Error(
                "Email.LinkGenerationFailed",
                "Unable to generate password reset link"));

        var messageBody = EmailTemplateHelpers.GetResetPasswordHtml(user.FirstName, verificationLink);
        await emailService.SendEmailAsync(user.Email, "Password Reset Request", messageBody, true);

        return Result.Success();
    }
}