using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.Users.Commands.Login
{
    public record LoginCommand(string Email, string Password) : ICommand<UserLoginRefreshDto>;
}
