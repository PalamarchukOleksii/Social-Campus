using Domain.Models;

namespace Application.Users.Commands.RegisterUserCommand
{
    public record RegisterUserCommandResponse(bool IsSuccess, User? User, string? ErrorMessage);
}
