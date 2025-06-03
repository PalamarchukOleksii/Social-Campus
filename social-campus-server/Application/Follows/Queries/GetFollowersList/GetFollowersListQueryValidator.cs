using FluentValidation;

namespace Application.Follows.Queries.GetFollowersList;

public class GetFollowersListQueryValidator : AbstractValidator<GetFollowersListQuery>
{
    public GetFollowersListQueryValidator()
    {
        RuleFor(f => f.Login)
            .NotEmpty().WithMessage("Login is required");
    }
}