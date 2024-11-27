using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Follows.Commands.Unfollow
{
    public class UnfollowCommandHandler(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IFollowRepository followRepository) : ICommandHandler<UnfollowCommand>
    {
        public async Task<Result> Handle(UnfollowCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByLoginAsync(request.UserLogin);
            if (user is null)
            {
                return Result.Failure(new Error(
                    "User.NotFound",
                    $"User with login {request.UserLogin} was not found"));
            }

            User? followUser = await userRepository.GetByLoginAsync(request.FollowUserLogin);
            if (followUser is null)
            {
                return Result.Failure(new Error(
                    "User.NotFound",
                    $"User with FollowUserId {request.FollowUserLogin} was not found"));
            }

            bool isAlreadyFollowing = await followRepository.IsFollowing(user.Id, followUser.Id);
            if (!isAlreadyFollowing)
            {
                return Result.Failure(new Error(
                    "Follow.NotFollowingYet",
                    $"User with UserId {user.Id.Value} was not following user with FollowUserId {followUser.Id.Value}")
                );
            }

            await followRepository.DeleteAsync(user.Id, followUser.Id);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
