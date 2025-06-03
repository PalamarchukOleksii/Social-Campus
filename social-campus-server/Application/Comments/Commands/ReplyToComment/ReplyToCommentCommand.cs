using Application.Abstractions.Messaging;
using Domain.Models.CommentModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.Comments.Commands.ReplyToComment;

public record ReplyToCommentCommand(
    PublicationId PublicationId,
    CommentId ReplyToCommentId,
    string Description,
    UserId CreatorId) : ICommand;