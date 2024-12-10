using FluentValidation;

namespace Application.Comments.Queries.GetRepliesToComment
{
    public class GetRepliesToCommentQueryValidator : AbstractValidator<GetRepliesToCommentQuery>
    {
        public GetRepliesToCommentQueryValidator()
        {
            RuleFor(x => x.CommentId)
                .NotNull().WithMessage("CommentId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("CommentId must be a valid GUID");
        }
    }
}
