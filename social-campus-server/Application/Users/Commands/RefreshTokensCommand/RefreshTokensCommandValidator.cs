using FluentValidation;

namespace Application.Users.Commands.RefreshTokensCommand
{
    public class RefreshTokensCommandValidator : AbstractValidator<RefreshTokensCommandRequest>
    {
        public RefreshTokensCommandValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("Access token is required.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MinimumLength(128).WithMessage("Refresh token format is invalid.");
        }
    }
}
