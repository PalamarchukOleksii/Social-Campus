using Application.Helpers;
using FluentValidation;

namespace Application.Users.Commands.Update
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required")
                .Must(email => ValidationHelpers.IsDomainValid(email))
                .WithMessage("Email domain must be one of the allowed domains")
                .MaximumLength(100).WithMessage("Email need to be shorter than 100 characters");

            RuleFor(u => u.Login)
                .NotEmpty().WithMessage("Login is required")
                .MaximumLength(50).WithMessage("Login need to be shorter than 50 characters");

            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name need to be shorter than 50 characters");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name need to be shorter than 50 characters");

            RuleFor(u => u.Bio)
                .MaximumLength(500).WithMessage("Bio need to be shorter than 500 characters");
        }
    }
}
