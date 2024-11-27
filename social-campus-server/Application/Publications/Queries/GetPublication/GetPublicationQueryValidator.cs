using FluentValidation;

namespace Application.Publications.Queries.GetPublication
{
    public class GetPublicationQueryValidator : AbstractValidator<GetPublicationQuery>
    {
        public GetPublicationQueryValidator()
        {
            RuleFor(f => f.PublicationId)
                .NotEmpty().WithMessage("PublicationId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("PublicationId must be a valid GUID");
        }
    }
}
