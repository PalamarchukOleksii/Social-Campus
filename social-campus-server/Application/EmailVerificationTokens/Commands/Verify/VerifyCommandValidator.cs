using FluentValidation;

namespace Application.EmailVerificationTokens.Commands.Verify;

public class VerifyCommandValidator : AbstractValidator<VerifyCommand>
{
    public VerifyCommandValidator()
    {
        RuleFor(vq => vq.Token)
            .NotEmpty().WithMessage("Token is required")
            .Must(token => token != Guid.Empty).WithMessage("Token must be a valid GUID");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email address is required")
            .MaximumLength(100).WithMessage("Email need to be shorter than 100 characters");
    }
}