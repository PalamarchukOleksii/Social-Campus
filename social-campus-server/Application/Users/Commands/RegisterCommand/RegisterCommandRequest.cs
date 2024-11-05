using MediatR;

namespace Application.Users.Commands.RegisterCommand
{
    public record RegisterCommandRequest(
        string Login,
        string FirstName,
        string LastName,
        string Email,
        string Password) : IRequest;
}
