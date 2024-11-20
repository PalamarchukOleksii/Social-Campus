using Application.Abstractions.Messaging;
using Domain.Models.UserModel;

namespace Application.Follows.Commands.Unfollow
{
    public record UnfollowCommand(UserId UserId, UserId FollowUserId) : ICommand;
}
