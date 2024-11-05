using MediatR;

namespace Application.Users.Commands.LoginCommand
{
    public record LoginCommandRequest(string Email, string Password) : IRequest<LoginCommandResponse>;
}
