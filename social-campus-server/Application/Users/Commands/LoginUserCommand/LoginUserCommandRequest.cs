using MediatR;

namespace Application.Users.Commands.LoginUserCommand
{
    public record LoginUserCommandRequest(string Email, string Password) : IRequest<LoginUserCommandResponse>;
}
