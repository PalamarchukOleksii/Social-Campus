using Application.Abstractions.Messaging;
using Domain.Dtos;

namespace Application.Users.Commands.Login
{
    public record LoginCommand(string Email, string Password) : ICommand<TokensDto>;
}
