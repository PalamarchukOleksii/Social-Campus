using FluentValidation;

namespace Application.ResetPasswordTokens.Commands.Verify;

public class VerifyCommandValidator : AbstractValidator<VerifyCommand>
{
    public VerifyCommandValidator()
    {
        RuleFor(vq => vq.Token)
            .NotEmpty().WithMessage("Token is required")
            .Must(token => token != Guid.Empty).WithMessage("Token must be a valid GUID");

        RuleFor(vq => vq.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");
    }
}