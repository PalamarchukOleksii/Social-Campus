using Application.Abstractions.Messaging;

namespace Application.Follows.Commands.Unfollow
{
    public record UnfollowCommand(string UserLogin, string FollowUserLogin) : ICommand;
}
