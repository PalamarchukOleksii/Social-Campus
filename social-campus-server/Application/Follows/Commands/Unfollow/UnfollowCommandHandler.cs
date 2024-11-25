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
            User? user = await userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                return Result.Failure(new Error(
                    "User.NotFound",
                    $"User with UserId {request.UserId} was not found"));
            }

            user = await userRepository.GetByIdAsync(request.FollowUserId);
            if (user is null)
            {
                return Result.Failure(new Error(
                    "User.NotFound",
                    $"User with FollowUserId {request.FollowUserId} was not found"));
            }

            bool isAlreadyFollowing = await followRepository.IsFollowing(request.UserId, request.FollowUserId);
            if (!isAlreadyFollowing)
            {
                return Result.Failure(new Error(
                    "Follow.NotFollowingYet",
                    $"User with UserId {request.UserId.Value} was not following user with FollowUserId {request.FollowUserId.Value}")
                );
            }

            await followRepository.DeleteAsync(request.UserId, request.FollowUserId);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
