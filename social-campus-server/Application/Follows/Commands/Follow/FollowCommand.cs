using Application.Abstractions.Messaging;

namespace Application.Follows.Commands.Follow
{
    public record FollowCommand(string UserLogin, string FollowUserLogin) : ICommand;
}
