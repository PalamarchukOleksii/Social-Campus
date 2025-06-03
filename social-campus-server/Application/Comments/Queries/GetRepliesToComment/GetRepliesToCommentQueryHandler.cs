using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Comments.Queries.GetRepliesToComment;

public class GetRepliesToCommentQueryHandler(
    ICommentRepository commentRepository) : IQueryHandler<GetRepliesToCommentQuery, IReadOnlyList<CommentDto>>
{
    public async Task<Result<IReadOnlyList<CommentDto>>> Handle(GetRepliesToCommentQuery request,
        CancellationToken cancellationToken)
    {
        var isCommentExist = await commentRepository.IsExistByIdAsync(request.CommentId);
        if (!isCommentExist)
            return Result.Failure<IReadOnlyList<CommentDto>>(new Error(
                "Comment.NotFound",
                $"Comment with id {request.CommentId.Value} was not found"));

        var comments = await commentRepository.GetRepliedCommentsByCommentIdAsync(request.CommentId);

        List<CommentDto> commentDtos = new();

        foreach (var c in comments)
            commentDtos.Add(new CommentDto
            {
                Id = c.Id,
                Description = c.Description,
                CreationDateTime = c.CreationDateTime,
                PublicationId = c.RelatedPublicationId,
                CreatorId = c.Creator!.Id,
                UserWhoLikedIds = c.CommentLikes?
                    .Select(like => like.UserId).ToList()
            });

        return Result.Success(commentDtos.ToList() as IReadOnlyList<CommentDto>);
    }
}