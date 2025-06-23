using FluentValidation;

namespace Application.Publications.Commands.DeletePublication;

public class DeletePublicationCommandValidator : AbstractValidator<DeletePublicationCommand>
{
    public DeletePublicationCommandValidator()
    {
        RuleFor(f => f.CallerId)
            .NotEmpty().WithMessage("CallerId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("CallerId must be a valid GUID");

        RuleFor(u => u.PublicationId)
            .NotEmpty().WithMessage("PublicationId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("PublicationId must be a valid GUID");
    }
}