using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.PublicationLikeModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Publications.Queries.GetPublication
{
    public class GetPublicationQueryHandler(
        IPublicationRepository publicationRepository,
        IUserRepository userRepository,
        IPublicationLikeRepositories publicationLikeRepositories,
        IFollowRepository followRepository) : IQueryHandler<GetPublicationQuery, ShortPublicationDto>
    {
        public async Task<Result<ShortPublicationDto>> Handle(GetPublicationQuery request, CancellationToken cancellationToken)
        {
            Publication? publication = await publicationRepository.GetByIdAsync(request.PublicationId);
            if (publication is null)
            {
                return Result.Failure<ShortPublicationDto>(new Error(
                    "Publication.NotFound",
                    $"Publication with PublicationId {request.PublicationId.Value} was not found"));
            }

            User? user = await userRepository.GetByIdAsync(publication.CreatorId);
            if (user is null)
            {
                return Result.Failure<ShortPublicationDto>(new Error(
                    "User.NotFound",
                    $"User with UserId {publication.CreatorId.Value} was not found"));
            }

            IReadOnlyList<PublicationLike> publicationLikes = await publicationLikeRepositories
                .GetPublicationLikesListByPublicationIdAsync(publication.Id);
            IReadOnlyList<User> followers = await followRepository.GetFollowersUsersByUserIdAsync(user.Id);

            ShortPublicationDto shortPublicationDto = new()
            {
                Id = publication.Id,
                Description = publication.Description,
                ImageData = publication.ImageData,
                CreationDateTime = publication.CreationDateTime,
                CreatorInfo = new ShortUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Login = user.Login,
                    Bio = user.Bio,
                    ProfileImageData = user.ProfileImageData,
                    FollowersIds = followers
                        .Select(f => f.Id)
                        .ToList() as IReadOnlyList<UserId>
                },
                UserWhoLikedIds = publicationLikes
                        .Select(like => like.UserId)
                        .ToList() as IReadOnlyList<UserId>
            };

            return Result.Success(shortPublicationDto);
        }
    }
}
