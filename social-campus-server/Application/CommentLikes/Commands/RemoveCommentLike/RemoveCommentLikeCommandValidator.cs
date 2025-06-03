using FluentValidation;

namespace Application.CommentLikes.Commands.RemoveCommentLike;

public class RemoveCommentLikeCommandValidator : AbstractValidator<RemoveCommentLikeCommand>
{
    public RemoveCommentLikeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("UserId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");

        RuleFor(x => x.CommentId)
            .NotNull().WithMessage("CommentId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("CommentId must be a valid GUID");
    }
}