using FluentValidation;

namespace Application.RefreshTokens.Commands.Revoke;

public class RevokeCommandValidator : AbstractValidator<RevokeCommand>
{
    public RevokeCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required")
            .MinimumLength(128).WithMessage("Refresh token format is invalid");
    }
}