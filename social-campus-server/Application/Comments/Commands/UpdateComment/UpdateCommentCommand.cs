using Application.Abstractions.Messaging;
using Domain.Models.CommentModel;
using Domain.Models.UserModel;

namespace Application.Comments.Commands.UpdateComment
{
    public record UpdateCommentCommand(UserId CallerId, CommentId CommentId, string Description) : ICommand;
}
