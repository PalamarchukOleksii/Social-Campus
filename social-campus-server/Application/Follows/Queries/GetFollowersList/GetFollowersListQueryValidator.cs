using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.Follows.Queries.GetFollowersList
{
    public class GetFollowersListQueryValidator : AbstractValidator<GetFollowersListQuery>
    {
        public GetFollowersListQueryValidator(IUserRepository userRepository)
        {
            RuleFor(f => f.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .MustAsync(async (userId, _) => await userRepository.IsExistByIdAsync(userId))
                .WithMessage((context, _) => $"User with UserId {context.UserId.Value} do not exist.");
        }
    }
}
