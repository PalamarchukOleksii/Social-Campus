using Domain.Repositories;
using FluentValidation;

namespace Application.Users.Commands.RegisterCommand
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommandRequest>
    {
        public RegisterCommandValidator(IUserRepository userRepository)
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MustAsync(async (email, _) => await userRepository.IsEmailUniqueAsync(email))
                .WithMessage("Email must be unique.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(u => u.Login)
                .NotEmpty().WithMessage("Login is required.")
                .MustAsync(async (login, _) => await userRepository.IsLoginUniqueAsync(login))
                .WithMessage("Login must be unique.");

            RuleFor(u => u.FirstName).NotEmpty().WithMessage("First name is required.");

            RuleFor(u => u.LastName).NotEmpty().WithMessage("Last name is required.");
        }
    }
}
