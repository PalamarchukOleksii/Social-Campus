using FluentValidation;

namespace Application.Publications.Commands.UpdatePublication;

public class UpdatePublicationCommandValidator : AbstractValidator<UpdatePublicationCommand>
{
    public UpdatePublicationCommandValidator()
    {
        RuleFor(f => f.CallerId)
            .NotEmpty().WithMessage("CallerId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("CallerId must be a valid GUID");

        RuleFor(f => f.PublicationId)
            .NotEmpty().WithMessage("PublicationId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("PublicationId must be a valid GUID");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");
    }
}