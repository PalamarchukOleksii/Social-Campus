using Application.Abstractions.Messaging;
using Domain.Models.UserModel;

namespace Application.Publications.Commands.Create
{
    public record CreatePublicationCommand(string Description, UserId CreatorId, string? Base64ImageData) : ICommand;
}
