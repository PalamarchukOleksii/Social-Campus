using FluentValidation;

namespace Application.Users.Queries.GetUserPublications;

public class GetUserPublicationsQueryValidator : AbstractValidator<GetUserPublicationsQuery>
{
    public GetUserPublicationsQueryValidator()
    {
        RuleFor(f => f.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");
    }
}