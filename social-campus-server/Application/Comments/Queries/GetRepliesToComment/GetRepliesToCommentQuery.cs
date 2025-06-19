using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.CommentModel;

namespace Application.Comments.Queries.GetRepliesToComment;

public record GetRepliesToCommentQuery(CommentId CommentId, int Page, int Count) : IQuery<IReadOnlyList<CommentDto>>;