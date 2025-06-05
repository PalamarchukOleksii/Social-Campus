using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.CommentModel;

namespace Application.Comments.Queries.GetComment;

public record GetCommentQuery(CommentId CommentId) : IQuery<CommentDto>;