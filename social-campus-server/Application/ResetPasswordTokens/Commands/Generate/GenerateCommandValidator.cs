using FluentValidation;

namespace Application.ResetPasswordTokens.Commands.Generate;

public class GenerateCommandValidator : AbstractValidator<GenerateCommand>
{
    public GenerateCommandValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email address is required")
            .MaximumLength(100).WithMessage("Email need to be shorter than 100 characters");
    }
}