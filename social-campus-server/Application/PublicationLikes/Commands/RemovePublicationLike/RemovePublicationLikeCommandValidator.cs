using FluentValidation;

namespace Application.PublicationLikes.Commands.RemovePublicationLike;

public class RemovePublicationLikeCommandValidator : AbstractValidator<RemovePublicationLikeCommand>
{
    public RemovePublicationLikeCommandValidator()
    {
        RuleFor(f => f.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");

        RuleFor(f => f.PublicationId)
            .NotEmpty().WithMessage("PublicationId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("PublicationId must be a valid GUID");
    }
}