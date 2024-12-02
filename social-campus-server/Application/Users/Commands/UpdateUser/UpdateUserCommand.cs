using Application.Abstractions.Messaging;
using Domain.Models.UserModel;

namespace Application.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(
        UserId CallerId,
        UserId UserId,
        string Login,
        string Email,
        string FirstName,
        string LastName,
        string Bio,
        string ProfileImageData) : ICommand;
}
