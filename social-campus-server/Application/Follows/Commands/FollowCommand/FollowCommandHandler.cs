using Application.Abstractions.Data;
using Domain.Abstractions.Repositories;
using MediatR;

namespace Application.Follows.Commands.FollowCommand
{
    public class FollowCommandHandler(
        IUnitOfWork unitOfWork,
        IFollowRepository followRepository) : IRequestHandler<FollowCommandRequest, FollowCommandResponse>
    {
        public async Task<FollowCommandResponse> Handle(FollowCommandRequest request, CancellationToken cancellationToken)
        {
            bool followAlreadyFollowing = await followRepository.IsFollowing(request.UserId, request.FollowUserId);
            if (followAlreadyFollowing)
            {
                return new FollowCommandResponse(
                    IsSuccess: false,
                    ErrorMessage: $"User with UserId {request.UserId.Value} already following user with FollowUserId {request.FollowUserId.Value}."
                );
            }

            await followRepository.AddAsync(request.UserId, request.FollowUserId);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new FollowCommandResponse(
                IsSuccess: true,
                ErrorMessage: default
            );
        }
    }
}
