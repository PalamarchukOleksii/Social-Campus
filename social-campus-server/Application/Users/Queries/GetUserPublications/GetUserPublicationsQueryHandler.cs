using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.PublicationModel;
using Domain.Shared;

namespace Application.Users.Queries.GetUserPublications;

public class GetUserPublicationsQueryHandler(
    IPublicationRepository publicationRepository,
    IUserRepository userRepository,
    IPublicationLikeRepository publicationLikeRepository,
    ICommentRepository commentRepository)
    : IQueryHandler<GetUserPublicationsQuery, IReadOnlyList<PublicationDto>>
{
    public async Task<Result<IReadOnlyList<PublicationDto>>> Handle(GetUserPublicationsQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure<IReadOnlyList<PublicationDto>>(new Error(
                "User.NotFound",
                $"User with UserId {request.UserId.Value} was not found"));

        Publication? lastPublication = null;
        if (request.LastPublicationId is not null)
        {
            lastPublication = await publicationRepository.GetByIdAsync(request.LastPublicationId);
            if (lastPublication is null)
                return Result.Failure<IReadOnlyList<PublicationDto>>(new Error(
                    "Publication.NotFound",
                    $"Publication with PublicationId {request.LastPublicationId.Value} was not found"));
        }

        var userPublications = await publicationRepository.GetUserPublicationsByUserIdAsync(
            user.Id,
            lastPublication,
            request.Count);

        var publicationDtos = new List<PublicationDto>();
        foreach (var publication in userPublications)
        {
            var publicationLikes = await publicationLikeRepository
                .GetPublicationLikesListByPublicationIdAsync(publication.Id);

            var comments = await commentRepository
                .GetPublicationCommentsByPublicationIdAsync(publication.Id);

            publicationDtos.Add(new PublicationDto
            {
                Id = publication.Id,
                Description = publication.Description,
                ImageUrl = publication.ImageUrl,
                CreationDateTime = publication.CreationDateTime,
                CreatorId = user.Id,
                UserWhoLikedIds = publicationLikes.Select(like => like.UserId).ToList(),
                CommentsCount = comments.Count
            });
        }

        return Result.Success<IReadOnlyList<PublicationDto>>(publicationDtos);
    }
}