using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.PublicationModel;
using Domain.Shared;

namespace Application.Publications.Queries.GetHomePagePublications;

public class GetHomePagePublicationsQueryHandler(
    IUserRepository userRepository,
    IPublicationRepository publicationRepository,
    IFollowRepository followRepository,
    ICommentRepository commentRepository,
    IPublicationLikeRepository publicationLikeRepository)
    : IQueryHandler<GetHomePagePublicationsQuery, IReadOnlyList<PublicationDto>>
{
    public async Task<Result<IReadOnlyList<PublicationDto>>> Handle(GetHomePagePublicationsQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure<IReadOnlyList<PublicationDto>>(new Error(
                "User.NotFound",
                $"User with UserId {request.UserId.Value} was not found"));


        var followedUsers = await followRepository.GetFollowingUsersByUserIdAsync(user.Id);

        var homePagePublications = await publicationRepository.GetPublicationsForHomePageAsync(
            followedUsers,
            request.Page,
            request.Count,
            user
        );

        var homePagePublicationsDtos = new List<PublicationDto>();
        foreach (var publication in homePagePublications)
        {
            var publicationLikes = await publicationLikeRepository
                .GetPublicationLikesListByPublicationIdAsync(publication.Id);

            var comments = await commentRepository
                .GetPublicationCommentsByPublicationIdAsync(publication.Id);

            homePagePublicationsDtos.Add(new PublicationDto
            {
                Id = publication.Id,
                Description = publication.Description,
                ImageUrl = publication.ImageUrl,
                CreationDateTime = publication.CreationDateTime,
                CreatorId = publication.CreatorId,
                UserWhoLikedIds = publicationLikes.Select(like => like.UserId).ToList(),
                CommentsCount = comments.Count
            });
        }

        return Result.Success<IReadOnlyList<PublicationDto>>(homePagePublicationsDtos);
    }
}