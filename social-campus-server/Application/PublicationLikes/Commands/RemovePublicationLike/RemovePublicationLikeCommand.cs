using Application.Abstractions.Messaging;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.PublicationLikes.Commands.RemovePublicationLike
{
    public record RemovePublicationLikeCommand(UserId UserId, PublicationId PublicationId) : ICommand;
}
