using FluentValidation;

namespace Application.Comments.Queries.GetRepliesToComment;

public class GetRepliesToCommentQueryValidator : AbstractValidator<GetRepliesToCommentQuery>
{
    public GetRepliesToCommentQueryValidator()
    {
        RuleFor(x => x.CommentId)
            .NotNull().WithMessage("CommentId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("CommentId must be a valid GUID");

        RuleFor(f => f.Page)
            .GreaterThan(0)
            .WithMessage("Count must be greater than 0.");

        RuleFor(x => x.Count)
            .GreaterThan(0)
            .WithMessage("Count must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Count must be 100 or less.");
    }
}