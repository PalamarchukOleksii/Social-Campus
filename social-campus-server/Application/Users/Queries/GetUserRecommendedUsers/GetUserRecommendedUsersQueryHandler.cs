using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Queries.GetUserRecommendedUsers;

public class GetUserRecommendedUsersQueryHandler(IUserRepository userRepository, IFollowRepository followRepository)
    : IQueryHandler<GetUserRecommendedUsersQuery, IReadOnlyList<UserDto>>
{
    public async Task<Result<IReadOnlyList<UserDto>>> Handle(GetUserRecommendedUsersQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure<IReadOnlyList<UserDto>>(new Error(
                "User.NotFound",
                $"User with userId {request.UserId} was not found"));

        var followers = await followRepository.GetFollowersUsersByUserIdAsync(user.Id);
        var following = await followRepository.GetFollowingUsersByUserIdAsync(user.Id);

        var followingIds = following.Select(x => x.Id).ToHashSet();

        const int recommendationsCount = 5;
        var random = new Random();

        var followersNotFollowedBack = followers
            .Where(f => !followingIds.Contains(f.Id) && f.Id != user.Id)
            .ToList();

        var recommendedUsers = followersNotFollowedBack
            .OrderBy(_ => random.Next())
            .Take(recommendationsCount)
            .ToList();

        if (recommendedUsers.Count < recommendationsCount)
        {
            var needed = recommendationsCount - recommendedUsers.Count;

            var excludedIds = new HashSet<Guid>(recommendedUsers.Select(u => u.Id.Value)) { user.Id.Value };

            var randomUsers =
                await userRepository.GetRandomUsersAsync(recommendationsCount * 2);
            var additionalUsers = randomUsers
                .Where(u => !excludedIds.Contains(u.Id.Value))
                .DistinctBy(u => u.Id)
                .Take(needed)
                .ToList();

            recommendedUsers.AddRange(additionalUsers);
        }

        var recommendationsDto = new List<UserDto>();

        foreach (var recommendedUser in recommendedUsers)
        {
            var userFollowers = await followRepository.GetFollowersUsersByUserIdAsync(recommendedUser.Id);

            recommendationsDto.Add(new UserDto
            {
                Id = recommendedUser.Id,
                Login = recommendedUser.Login,
                FirstName = recommendedUser.FirstName,
                LastName = recommendedUser.LastName,
                Bio = recommendedUser.Bio,
                ProfileImageUrl = recommendedUser.ProfileImageUrl,
                FollowersIds = userFollowers.Select(f => f.Id).ToList()
            });
        }

        return Result.Success<IReadOnlyList<UserDto>>(recommendationsDto);
    }
}