using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Comments.Queries.GetComment;

public class GetCommentQueryHandler(ICommentRepository commentRepository, ICommentLikeRepository commentLikeRepository)
    : IQueryHandler<GetCommentQuery, CommentDto>
{
    public async Task<Result<CommentDto>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(request.CommentId);
        if (comment is null)
            return Result.Failure<CommentDto>(new Error(
                "Comment.NotFound",
                $"Comment with CommentId {request.CommentId.Value} was not found"));

        var commentLikes = await commentLikeRepository
            .GetCommentLikesListByCommentIdAsync(comment.Id);

        var commentReplies = await commentRepository.GetRepliedCommentsByCommentIdAsync(comment.Id);

        CommentDto commentDto = new()
        {
            Id = comment.Id,
            Description = comment.Description,
            CreationDateTime = comment.CreationDateTime,
            CreatorId = comment.CreatorId,
            PublicationId = comment.RelatedPublicationId,
            UserWhoLikedIds = commentLikes
                .Select(like => like.UserId)
                .ToList(),
            RepliesCount = commentReplies.Count
        };

        return Result.Success(commentDto);
    }
}