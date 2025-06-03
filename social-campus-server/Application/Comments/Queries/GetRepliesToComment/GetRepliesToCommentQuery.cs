using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.CommentModel;

namespace Application.Comments.Queries.GetRepliesToComment;

public record GetRepliesToCommentQuery(CommentId CommentId) : IQuery<IReadOnlyList<CommentDto>>;