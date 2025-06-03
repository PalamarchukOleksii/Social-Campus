using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Follows.Commands.Follow;

public class FollowCommandHandler(
    IUserRepository userRepository,
    IFollowRepository followRepository) : ICommandHandler<FollowCommand>
{
    public async Task<Result> Handle(FollowCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByLoginAsync(request.UserLogin);
        if (user is null)
            return Result.Failure(new Error(
                "User.NotFound",
                $"User with login {request.UserLogin} was not found"));

        var followUser = await userRepository.GetByLoginAsync(request.FollowUserLogin);
        if (followUser is null)
            return Result.Failure(new Error(
                "User.NotFound",
                $"User with FollowUserId {request.FollowUserLogin} was not found"));

        var isAlreadyFollowing = await followRepository.IsFollowing(user.Id, followUser.Id);
        if (isAlreadyFollowing)
            return Result.Failure(new Error(
                "Follow.AlreadyFollowing",
                $"User with UserId {user.Id.Value} already following user with FollowUserId {followUser.Id.Value}"));

        await followRepository.AddAsync(user.Id, followUser.Id);

        return Result.Success();
    }
}