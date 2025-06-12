using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Queries.GetPublication;

public class GetPublicationQueryHandler(
    IPublicationRepository publicationRepository,
    IPublicationLikeRepositories publicationLikeRepositories,
    ICommentRepository commentRepository) : IQueryHandler<GetPublicationQuery, PublicationDto>
{
    public async Task<Result<PublicationDto>> Handle(GetPublicationQuery request, CancellationToken cancellationToken)
    {
        var publication = await publicationRepository.GetByIdAsync(request.PublicationId);
        if (publication is null)
            return Result.Failure<PublicationDto>(new Error(
                "Publication.NotFound",
                $"Publication with PublicationId {request.PublicationId.Value} was not found"));

        var publicationLikes = await publicationLikeRepositories
            .GetPublicationLikesListByPublicationIdAsync(publication.Id);
        var comments = await commentRepository.GetPublicationCommentsByPublicationIdAsync(publication.Id);

        PublicationDto publicationDto = new()
        {
            Id = publication.Id,
            Description = publication.Description,
            ImageUrl = publication.ImageUrl,
            CreationDateTime = publication.CreationDateTime,
            CreatorId = publication.CreatorId,
            UserWhoLikedIds = publicationLikes
                .Select(like => like.UserId)
                .ToList(),
            CommentsCount = comments.Count
        };

        return Result.Success(publicationDto);
    }
}