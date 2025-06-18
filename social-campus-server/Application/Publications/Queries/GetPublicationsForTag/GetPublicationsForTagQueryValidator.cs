using FluentValidation;

namespace Application.Publications.Queries.GetPublicationsForTag;

public class GetPublicationsForTagQueryValidator : AbstractValidator<GetPublicationsForTagQuery>
{
    public GetPublicationsForTagQueryValidator()
    {
        RuleFor(x => x.TagLabel)
            .NotEmpty().WithMessage("TagLabel is required");

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