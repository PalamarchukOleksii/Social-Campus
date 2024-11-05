using FluentValidation;

namespace Application.RefreshTokens.Commands.RevokeCommand
{
    public class RevokeCommandValidator : AbstractValidator<RevokeCommandRequest>
    {
        public RevokeCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MinimumLength(128).WithMessage("Refresh token format is invalid.");
        }
    }
}
