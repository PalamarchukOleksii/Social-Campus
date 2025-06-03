using FluentValidation;

namespace Application.Comments.Commands.ReplyToComment;

public class ReplyToCommentCommandValidator : AbstractValidator<ReplyToCommentCommand>
{
    public ReplyToCommentCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.ReplyToCommentId)
            .NotNull().WithMessage("ReplyToCommentId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("ReplyToCommentId must be a valid GUID");

        RuleFor(x => x.CreatorId)
            .NotNull().WithMessage("CreatorId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("CreatorId must be a valid GUID");

        RuleFor(x => x.PublicationId)
            .NotNull().WithMessage("PublicationId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("PublicationId must be a valid GUID");
    }
}