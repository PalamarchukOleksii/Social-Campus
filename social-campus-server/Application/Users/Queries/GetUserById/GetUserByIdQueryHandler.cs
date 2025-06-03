using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandlerI(
    IUserRepository userRepository,
    IFollowRepository followRepository) : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure<UserDto>(new Error(
                "User.NotFound",
                $"User with UserId {request.UserId.Value} was not found"));

        var following = await followRepository.GetFollowingUsersByUserIdAsync(user.Id);
        var followers = await followRepository.GetFollowersUsersByUserIdAsync(user.Id);

        UserDto userProfile = new()
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Bio = user.Bio,
            ProfileImageUrl = user.ProfileImageUrl,
            FollowersCount = followers.Count,
            FollowingCount = following.Count,
            FollowersIds = followers
                .Select(f => f.Id)
                .ToList()
        };

        return Result.Success(userProfile);
    }
}