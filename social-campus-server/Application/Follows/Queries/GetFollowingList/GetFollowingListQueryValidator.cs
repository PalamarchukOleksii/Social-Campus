using FluentValidation;

namespace Application.Follows.Queries.GetFollowingList
{
    public class GetFollowingListQueryValidator : AbstractValidator<GetFollowingListQuery>
    {
        public GetFollowingListQueryValidator()
        {
            RuleFor(f => f.UserId)
                .NotEmpty().WithMessage("UserId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");
        }
    }
}
