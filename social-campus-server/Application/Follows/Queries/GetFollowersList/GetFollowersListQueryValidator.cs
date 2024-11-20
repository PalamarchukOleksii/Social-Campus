﻿using Domain.Abstractions.Repositories;
using FluentValidation;

namespace Application.Follows.Queries.GetFollowersList
{
    public class GetFollowersListQueryValidator : AbstractValidator<GetFollowersListQuery>
    {
        public GetFollowersListQueryValidator(IUserRepository userRepository)
        {
            RuleFor(f => f.UserId)
                .NotEmpty().WithMessage("UserId is required")
                .Must(id => id.Value != Guid.Empty).WithMessage("UserId must be a valid GUID")
                .MustAsync(async (userId, _) => await userRepository.IsExistByIdAsync(userId))
                .WithMessage((context, _) => $"User with UserId {context.UserId.Value} do not exist");
        }
    }
}
