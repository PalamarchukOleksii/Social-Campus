using FluentValidation;

namespace Application.Publications.Queries.GetPublicationComments;

public class GetPublicationCommentsQueryValidator : AbstractValidator<GetPublicationCommentsQuery>
{
    public GetPublicationCommentsQueryValidator()
    {
        RuleFor(f => f.PublicationId)
            .NotEmpty().WithMessage("PublicationId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("PublicationId must be a valid GUID");

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