using Domain.Models.UserModel;
using MediatR;

namespace Application.Follows.Commands.UnfollowCommand
{
    public record UnfollowCommandRequest(UserId UserId, UserId FollowUserId) : IRequest<UnfollowCommandResponse>;
}
