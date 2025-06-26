using FluentValidation;

namespace Application.Users.Commands.VerifyEmail;

public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(f => f.EmailVerificationTokenId)
            .NotEmpty().WithMessage("EmailVerificationTokenId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("EmailVerificationTokenId must be a valid GUID");
    }
}