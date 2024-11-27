using Application.Abstractions.Messaging;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.PublicationLikes.Commands.AddPublicationLike
{
    public record AddPublicationLikeCommand(UserId UserId, PublicationId PublicationId) : ICommand;
}
