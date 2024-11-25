using FluentValidation;

namespace Application.Follows.Queries.GetFollowersList
{
    public class GetFollowersListQueryValidator : AbstractValidator<GetFollowersListQuery>
    {
        public GetFollowersListQueryValidator()
        {
            RuleFor(f => f.UserId)
                .NotEmpty().WithMessage("UserId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");
        }
    }
}
