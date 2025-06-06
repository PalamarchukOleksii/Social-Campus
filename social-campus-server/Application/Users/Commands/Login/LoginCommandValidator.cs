using FluentValidation;

namespace Application.Users.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email address is required")
            .WithMessage("Email domain must be one of the allowed domains");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}