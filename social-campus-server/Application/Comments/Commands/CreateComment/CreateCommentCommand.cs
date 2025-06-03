using Application.Abstractions.Messaging;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.Comments.Commands.CreateComment;

public record CreateCommentCommand(
    PublicationId PublicationId,
    string Description,
    UserId CreatorId) : ICommand;