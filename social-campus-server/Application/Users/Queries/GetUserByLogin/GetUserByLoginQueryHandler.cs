using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Users.Queries.GetUserByLogin;

public class GetUserProfileByLoginHandler(
    IUserRepository userRepository,
    IFollowRepository followRepository,
    IStorageService storageService) : IQueryHandler<GetUserByLoginQuery, UserDto>
{
    public async Task<Result<UserDto>> Handle(GetUserByLoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByLoginAsync(request.Login);
        if (user is null)
            return Result.Failure<UserDto>(new Error(
                "User.NotFound",
                $"User with login {request.Login} was not found"));

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
            ProfileImageUrl = await storageService.GetPresignedUrlAsync(user.ProfileImageObjectKey),
            FollowersCount = followers.Count,
            FollowingCount = following.Count,
            FollowersIds = followers
                .Select(f => f.Id)
                .ToList()
        };

        return Result.Success(userProfile);
    }
}