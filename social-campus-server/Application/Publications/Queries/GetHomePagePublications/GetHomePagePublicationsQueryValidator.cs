using FluentValidation;

namespace Application.Publications.Queries.GetHomePagePublications;

public class GetHomePagePublicationsQueryValidator : AbstractValidator<GetHomePagePublicationsQuery>
{
    public GetHomePagePublicationsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");

        RuleFor(f => f.LastPublicationId)
            .Must(id => id == null || id.Value != Guid.Empty).WithMessage("PublicationId must be a valid GUID");

        RuleFor(x => x.Limit)
            .GreaterThan(0)
            .WithMessage("Limit must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Limit must be 100 or less.");
    }
}