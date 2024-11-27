using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Follows.Commands.Follow
{
    public class FollowCommandHandler(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IFollowRepository followRepository) : ICommandHandler<FollowCommand>
    {
        public async Task<Result> Handle(FollowCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                return Result.Failure(new Error(
                    "User.NotFound",
                    $"User with UserId {request.UserId.Value} was not found"));
            }

            user = await userRepository.GetByIdAsync(request.FollowUserId);
            if (user is null)
            {
                return Result.Failure(new Error(
                    "User.NotFound",
                    $"User with FollowUserId {request.FollowUserId.Value} was not found"));
            }

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
