using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Follows.Queries.GetFollowingList;

public class GetFollowingListQueryHandler(
    IFollowRepository followRepository,
    IUserRepository userRepository,
    IStorageService storageService) : IQueryHandler<GetFollowingListQuery, IReadOnlyList<UserDto>>
{
    public async Task<Result<IReadOnlyList<UserDto>>> Handle(GetFollowingListQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByLoginAsync(request.Login);
        if (user is null)
            return Result.Failure<IReadOnlyList<UserDto>>(new Error(
                "User.NotFound",
                $"User with login {request.Login} was not found"));

        var response = await followRepository.GetFollowingUsersByUserIdAsync(user.Id);

        List<UserDto> followingDto = [];

        foreach (var followedUser in response)
        {
            var followers = await followRepository.GetFollowersUsersByUserIdAsync(followedUser.Id);

            followingDto.Add(new UserDto
            {
                Id = followedUser.Id,
                Login = followedUser.Login,
                FirstName = followedUser.FirstName,
                LastName = followedUser.LastName,
                Bio = followedUser.Bio,
                ProfileImageUrl = await storageService.GetPresignedUrlAsync(followedUser.ProfileImageObjectKey),
                FollowersIds = followers.Select(f => f.Id).ToList()
            });
        }

        return Result.Success<IReadOnlyList<UserDto>>(followingDto);
    }
}