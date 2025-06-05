using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Queries.GetPublicationComments;

public class GetPublicationCommentsQueryHandler(
    IPublicationRepository publicationRepository,
    ICommentLikeRepository commentLikeRepository,
    ICommentRepository commentRepository)
    : IQueryHandler<GetPublicationCommentsQuery, IReadOnlyList<CommentDto>>
{
    public async Task<Result<IReadOnlyList<CommentDto>>> Handle(GetPublicationCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var publication = await publicationRepository.GetByIdAsync(request.PublicationId);
        if (publication is null)
            return Result.Failure<IReadOnlyList<CommentDto>>(new Error(
                "Publication.NotFound",
                $"Publication with PublicationId {request.PublicationId.Value} was not found"));

        var publicationComments = await commentRepository.GetPublicationCommentsByPublicationIdAsync(publication.Id);

        var commentDtos = new List<CommentDto>();
        foreach (var comment in publicationComments)
        {
            var commentLikes = await commentLikeRepository.GetCommentLikesListByCommentIdAsync(comment.Id);
            var commentReplies = await commentRepository.GetRepliedCommentsByCommentIdAsync(comment.Id);

            commentDtos.Add(new CommentDto
            {
                Id = comment.Id,
                Description = comment.Description,
                CreationDateTime = comment.CreationDateTime,
                CreatorId = comment.CreatorId,
                PublicationId = comment.RelatedPublicationId,
                UserWhoLikedIds = commentLikes.Select(like => like.UserId).ToList(),
                RepliesCount = commentReplies.Count
            });
        }

        return Result.Success<IReadOnlyList<CommentDto>>(commentDtos);
    }
}