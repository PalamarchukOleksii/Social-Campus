using Application.Abstractions.Messaging;
using Domain.Models.UserModel;

namespace Application.Users.Commands.Update
{
    public record UpdateUserCommand(
        UserId Id,
        string Login,
        string Email,
        string FirstName,
        string LastName,
        string Bio,
        string ProfileImageData) : ICommand;
}
