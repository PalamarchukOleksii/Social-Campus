using FluentValidation;

namespace Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(f => f.CallerId)
            .NotEmpty().WithMessage("CallerId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("CallerId must be a valid GUID");

        RuleFor(u => u.CommentId)
            .NotEmpty().WithMessage("CommentId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("CommentId must be a valid GUID");
    }
}