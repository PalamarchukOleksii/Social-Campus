using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Queries.GetPublicationsForTag;

public class GetPublicationsForTagQueryHandler(
    ITagRepository tagRepository,
    IPublicationTagRepository publicationTagRepository,
    ICommentRepository commentRepository,
    IPublicationLikeRepository publicationLikeRepository,
    IStorageService storageService) : IQueryHandler<GetPublicationsForTagQuery, IReadOnlyList<PublicationDto>>
{
    public async Task<Result<IReadOnlyList<PublicationDto>>> Handle(GetPublicationsForTagQuery request,
        CancellationToken cancellationToken)
    {
        var tag = await tagRepository.GetByLabelAsync(request.TagLabel);
        if (tag is null)
            return Result.Failure<IReadOnlyList<PublicationDto>>(new Error(
                "Tag.NotFound",
                $"Tag with TagLabel {request.TagLabel} was not found"));

        var publicationsForTag = await publicationTagRepository.GetPublicationsForTagAsync(
            tag.Id, request.Page, request.Count, cancellationToken
        );

        var publicationsForTagDtos = new List<PublicationDto>();
        foreach (var publication in publicationsForTag)
        {
            var publicationLikes = await publicationLikeRepository
                .GetPublicationLikesListByPublicationIdAsync(publication.Id);

            var comments = await commentRepository
                .GetPublicationCommentsByPublicationIdAsync(publication.Id);

            publicationsForTagDtos.Add(new PublicationDto
            {
                Id = publication.Id,
                Description = publication.Description,
                ImageUrl = await storageService.GetPresignedUrlAsync(publication.ImageObjectKey),
                CreationDateTime = publication.CreationDateTime,
                CreatorId = publication.CreatorId,
                UserWhoLikedIds = publicationLikes.Select(like => like.UserId).ToList(),
                CommentsCount = comments.Count
            });
        }

        return Result.Success<IReadOnlyList<PublicationDto>>(publicationsForTagDtos);
    }
}