﻿using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Queries.GetPublication;

public class GetPublicationQueryHandler(
    IPublicationRepository publicationRepository,
    IPublicationLikeRepository publicationLikeRepository,
    ICommentRepository commentRepository,
    IStorageService storageService) : IQueryHandler<GetPublicationQuery, PublicationDto>
{
    public async Task<Result<PublicationDto>> Handle(GetPublicationQuery request, CancellationToken cancellationToken)
    {
        var publication = await publicationRepository.GetByIdAsync(request.PublicationId);
        if (publication is null)
            return Result.Failure<PublicationDto>(new Error(
                "Publication.NotFound",
                $"Publication with PublicationId {request.PublicationId.Value} was not found"));

        var publicationLikes = await publicationLikeRepository
            .GetPublicationLikesListByPublicationIdAsync(publication.Id);
        var comments = await commentRepository.GetPublicationCommentsByPublicationIdAsync(publication.Id);

        PublicationDto publicationDto = new()
        {
            Id = publication.Id,
            Description = publication.Description,
            ImageUrl = await storageService.GetPresignedUrlAsync(publication.ImageObjectKey),
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