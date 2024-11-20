﻿using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.Follows.Commands.Unfollow
{
    public class UnfollowCommandValidator : AbstractValidator<UnfollowCommand>
    {
        public UnfollowCommandValidator(IUserRepository userRepository)
        {
            RuleFor(f => f.UserId)
                .NotEmpty().WithMessage("UserId is required")
                .MustAsync(async (userId, _) => await userRepository.IsExistByIdAsync(userId))
                .WithMessage((context, _) => $"User with UserId {context.UserId.Value} do not exist");

            RuleFor(f => f.FollowUserId)
                .NotEmpty().WithMessage("FollowUserId is required")
                .MustAsync(async (followUserId, _) => await userRepository.IsExistByIdAsync(followUserId))
                .WithMessage((context, _) => $"User with FollowUserId {context.FollowUserId.Value} do not exist");

            RuleFor(f => new { f.UserId, f.FollowUserId })
                .Must(x => x.UserId != x.FollowUserId)
                .WithMessage("UserId and FollowUserId must be different");
        }
    }
}
