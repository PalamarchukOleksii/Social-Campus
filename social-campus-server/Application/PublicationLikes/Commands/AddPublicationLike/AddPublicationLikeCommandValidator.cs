using FluentValidation;

namespace Application.PublicationLikes.Commands.AddPublicationLike;

public class AddPublicationLikeCommandValidator : AbstractValidator<AddPublicationLikeCommand>
{
    public AddPublicationLikeCommandValidator()
    {
        RuleFor(f => f.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");

        RuleFor(f => f.PublicationId)
            .NotEmpty().WithMessage("PublicationId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("PublicationId must be a valid GUID");
    }
}