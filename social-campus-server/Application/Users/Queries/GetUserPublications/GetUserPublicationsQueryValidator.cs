using FluentValidation;

namespace Application.Users.Queries.GetUserPublications;

public class GetUserPublicationsQueryValidator : AbstractValidator<GetUserPublicationsQuery>
{
    public GetUserPublicationsQueryValidator()
    {
        RuleFor(f => f.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");

        RuleFor(f => f.LastPublicationId)
            .Must(id => id == null || id.Value != Guid.Empty).WithMessage("PublicationId must be a valid GUID");

        RuleFor(x => x.Count)
            .GreaterThan(0)
            .WithMessage("Count must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Count must be 100 or less.");
    }
}