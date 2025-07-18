﻿using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Follows.Queries.GetFollowersList;

public class GetFollowersListQueryHandler(
    IFollowRepository followRepository,
    IUserRepository userRepository,
    IStorageService storageService) : IQueryHandler<GetFollowersListQuery, IReadOnlyList<UserDto>>
{
    public async Task<Result<IReadOnlyList<UserDto>>> Handle(GetFollowersListQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByLoginAsync(request.Login);
        if (user is null)
            return Result.Failure<IReadOnlyList<UserDto>>(new Error(
                "User.NotFound",
                $"User with login {request.Login} was not found"));

        var response = await followRepository.GetFollowersUsersByUserIdAsync(user.Id);

        List<UserDto> followersDto = [];

        foreach (var followingUser in response)
        {
            var userFollowers = await followRepository.GetFollowersUsersByUserIdAsync(followingUser.Id);

            followersDto.Add(new UserDto
            {
                Id = followingUser.Id,
                Login = followingUser.Login,
                FirstName = followingUser.FirstName,
                LastName = followingUser.LastName,
                Bio = followingUser.Bio,
                ProfileImageUrl = await storageService.GetPresignedUrlAsync(followingUser.ProfileImageObjectKey),
                FollowersIds = userFollowers
                    .Select(f => f.Id)
                    .ToList()
            });
        }

        return Result.Success<IReadOnlyList<UserDto>>(followersDto);
    }
}