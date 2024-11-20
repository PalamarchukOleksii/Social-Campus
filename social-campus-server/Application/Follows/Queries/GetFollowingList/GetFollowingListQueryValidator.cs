using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.Follows.Queries.GetFollowingList
{
    public class GetFollowingListQueryValidator : AbstractValidator<GetFollowingListQuery>
    {
        public GetFollowingListQueryValidator(IUserRepository userRepository)
        {
            RuleFor(f => f.UserId)
                .NotEmpty().WithMessage("UserId is required")
                .MustAsync(async (userId, _) => await userRepository.IsExistByIdAsync(userId))
                .WithMessage((context, _) => $"User with UserId {context.UserId.Value} do not exist");
        }
    }
}
