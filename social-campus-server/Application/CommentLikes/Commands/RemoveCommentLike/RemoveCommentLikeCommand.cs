using Application.Abstractions.Messaging;
using Domain.Models.CommentModel;
using Domain.Models.UserModel;

namespace Application.CommentLikes.Commands.RemoveCommentLike;

public record RemoveCommentLikeCommand(UserId UserId, CommentId CommentId) : ICommand;