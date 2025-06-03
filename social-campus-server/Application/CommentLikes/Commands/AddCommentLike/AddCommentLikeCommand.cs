using Application.Abstractions.Messaging;
using Domain.Models.CommentModel;
using Domain.Models.UserModel;

namespace Application.CommentLikes.Commands.AddCommentLike;

public record AddCommentLikeCommand(UserId UserId, CommentId CommentId) : ICommand;