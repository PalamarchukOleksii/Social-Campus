using Domain.Models.UserModel;
using MediatR;

namespace Application.Follows.Commands.FollowCommand
{
    public record FollowCommandRequest(UserId UserId, UserId FollowUserId) : IRequest<FollowCommandResponse>;
}
