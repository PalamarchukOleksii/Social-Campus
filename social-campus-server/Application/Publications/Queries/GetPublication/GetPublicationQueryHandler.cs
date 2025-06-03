using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Shared;

namespace Application.Publications.Queries.GetPublication;

public class GetPublicationQueryHandler(
    IPublicationRepository publicationRepository,
    IUserRepository userRepository,
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

        var user = await userRepository.GetByIdAsync(publication.CreatorId);
        if (user is null)
            return Result.Failure<PublicationDto>(new Error(
                "User.NotFound",
                $"User with UserId {publication.CreatorId.Value} was not found"));

        var publicationLikes = await publicationLikeRepositories
            .GetPublicationLikesListByPublicationIdAsync(publication.Id);
        var comments = await commentRepository.GetPublicationCommentsByPublicationIdAsync(publication.Id);

        PublicationDto publicationDto = new()
        {
            Id = publication.Id,
            Description = publication.Description,
            ImageData = publication.ImageData,
            CreationDateTime = publication.CreationDateTime,
            CreatorId = user.Id,
            UserWhoLikedIds = publicationLikes
                .Select(like => like.UserId)
                .ToList(),
            CommentsCount = comments.Count
        };

        return Result.Success(publicationDto);
    }
}