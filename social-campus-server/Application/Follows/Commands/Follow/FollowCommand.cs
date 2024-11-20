using Application.Abstractions.Messaging;
using Domain.Models.UserModel;

namespace Application.Follows.Commands.Follow
{
    public record FollowCommand(UserId UserId, UserId FollowUserId) : ICommand;
}
