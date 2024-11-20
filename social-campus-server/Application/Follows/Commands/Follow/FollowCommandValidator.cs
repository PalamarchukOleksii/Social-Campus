using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.Follows.Commands.Follow
{
    public class FollowCommandValidator : AbstractValidator<FollowCommand>
    {
        public FollowCommandValidator(IUserRepository userRepository)
        {
            RuleFor(f => f.UserId)
                .NotEmpty().WithMessage("UserId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID")
                .MustAsync(async (userId, _) => await userRepository.IsExistByIdAsync(userId))
                .WithMessage((context, _) => $"User with UserId {context.UserId.Value} do not exist");

            RuleFor(f => f.FollowUserId)
                .NotEmpty().WithMessage("FollowUserId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("FollowUserId must be a valid GUID")
                .MustAsync(async (followUserId, _) => await userRepository.IsExistByIdAsync(followUserId))
                .WithMessage((context, _) => $"User with FollowUserId {context.FollowUserId.Value} do not exist");

            RuleFor(f => new { f.UserId, f.FollowUserId })
                .Must(x => x.UserId != x.FollowUserId)
                .WithMessage("UserId and FollowUserId must be different");
        }
    }
}
