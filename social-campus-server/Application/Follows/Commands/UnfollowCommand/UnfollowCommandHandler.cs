using Application.Abstractions.Data;
using Domain.Abstractions.Repositories;
using MediatR;

namespace Application.Follows.Commands.UnfollowCommand
{
    public class UnfollowCommandHandler(
            IUnitOfWork unitOfWork,
            IFollowRepository followRepository) : IRequestHandler<UnfollowCommandRequest, UnfollowCommandResponse>
    {
        public async Task<UnfollowCommandResponse> Handle(UnfollowCommandRequest request, CancellationToken cancellationToken)
        {
            bool isAlreadyFollowing = await followRepository.IsFollowing(request.UserId, request.FollowUserId);
            if (!isAlreadyFollowing)
            {
                return new UnfollowCommandResponse(
                    IsSuccess: false,
                    ErrorMessage: $"User with UserId {request.UserId.Value} do not follow user with FollowUserId {request.FollowUserId.Value}."
                );
            }

            await followRepository.DeleteAsync(request.UserId, request.FollowUserId);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new UnfollowCommandResponse(
                IsSuccess: true,
                ErrorMessage: default
            );
        }
    }
}
