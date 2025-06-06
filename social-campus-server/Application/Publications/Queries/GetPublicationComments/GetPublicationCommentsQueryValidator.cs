using FluentValidation;

namespace Application.Publications.Queries.GetPublicationComments;

public class GetPublicationCommentsQueryValidator : AbstractValidator<GetPublicationCommentsQuery>
{
    public GetPublicationCommentsQueryValidator()
    {
        RuleFor(f => f.PublicationId)
            .NotEmpty().WithMessage("PublicationId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("PublicationId must be a valid GUID");
    }
}