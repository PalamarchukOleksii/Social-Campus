using FluentValidation;

namespace Application.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(f => f.CallerId)
            .NotEmpty().WithMessage("CallerId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("CallerId must be a valid GUID");

        RuleFor(u => u.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");
    }
}