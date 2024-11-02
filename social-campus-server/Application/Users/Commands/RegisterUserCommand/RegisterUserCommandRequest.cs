using MediatR;

namespace Application.Users.Commands.RegisterUserCommand
{
    public record RegisterUserCommandRequest(
        string Login,
        string FirstName,
        string LastName,
        string Email, string
        Password) : IRequest<RegisterUserCommandResponse>;
}
