using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Follows.Commands.Unfollow
{
    public class UnfollowCommandHandler(
            IUnitOfWork unitOfWork,
            IFollowRepository followRepository) : ICommandHandler<UnfollowCommand>
    {
        public async Task<Result> Handle(UnfollowCommand request, CancellationToken cancellationToken)
        {
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
