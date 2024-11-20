using Application.Abstractions.Messaging;
using Domain.Models.TokensModel;

namespace Application.Users.Commands.Login
{
    public record LoginCommand(string Email, string Password) : ICommand<Tokens>;
}
