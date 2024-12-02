using FluentValidation;

namespace Application.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description must not exceed 1000 characters");

            RuleFor(x => x.CallerId)
                .NotNull().WithMessage("CallerId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("CallerId must be a valid GUID");

            RuleFor(x => x.CommentId)
                .NotNull().WithMessage("CommentId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("CommentId must be a valid GUID");
        }
    }
}
