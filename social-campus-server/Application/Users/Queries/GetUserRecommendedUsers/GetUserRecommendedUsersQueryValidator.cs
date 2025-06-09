using FluentValidation;

namespace Application.Users.Queries.GetUserRecommendedUsers;

public class GetUserRecommendedUsersQueryValidator : AbstractValidator<GetUserRecommendedUsersQuery>
{
    public GetUserRecommendedUsersQueryValidator()
    {
        RuleFor(f => f.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID");
    }
}