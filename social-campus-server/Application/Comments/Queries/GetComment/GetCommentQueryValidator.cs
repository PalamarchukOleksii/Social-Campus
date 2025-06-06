using FluentValidation;

namespace Application.Comments.Queries.GetComment;

public class GetCommentQueryValidator : AbstractValidator<GetCommentQuery>
{
    public GetCommentQueryValidator()
    {
        RuleFor(f => f.CommentId)
            .NotEmpty().WithMessage("CommentId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("CommentId must be a valid GUID");
    }
}