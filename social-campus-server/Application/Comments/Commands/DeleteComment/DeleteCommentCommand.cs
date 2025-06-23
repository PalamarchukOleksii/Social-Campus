using Application.Abstractions.Messaging;
using Domain.Models.CommentModel;
using Domain.Models.UserModel;

namespace Application.Comments.Commands.DeleteComment;

public record DeleteCommentCommand(CommentId CommentId, UserId CallerId) : ICommand;