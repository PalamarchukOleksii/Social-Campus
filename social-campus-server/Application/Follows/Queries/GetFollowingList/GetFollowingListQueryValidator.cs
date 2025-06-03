using FluentValidation;

namespace Application.Follows.Queries.GetFollowingList;

public class GetFollowingListQueryValidator : AbstractValidator<GetFollowingListQuery>
{
    public GetFollowingListQueryValidator()
    {
        RuleFor(f => f.Login)
            .NotEmpty().WithMessage("Login is required");
    }
}