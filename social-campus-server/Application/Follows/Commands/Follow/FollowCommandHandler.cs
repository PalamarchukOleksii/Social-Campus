using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Follows.Commands.Follow
{
    public class FollowCommandHandler(
        IUnitOfWork unitOfWork,
        IFollowRepository followRepository) : ICommandHandler<FollowCommand>
    {
        public async Task<Result> Handle(FollowCommand request, CancellationToken cancellationToken)
        {
            bool isAlreadyFollowing = await followRepository.IsFollowing(request.UserId, request.FollowUserId);
            if (isAlreadyFollowing)
            {
                return Result.Failure(new Error(
                    "Follow.AlreadyFollowing",
                    $"User with UserId {request.UserId.Value} already following user with FollowUserId {request.FollowUserId.Value}"));
            }

            await followRepository.AddAsync(request.UserId, request.FollowUserId);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
