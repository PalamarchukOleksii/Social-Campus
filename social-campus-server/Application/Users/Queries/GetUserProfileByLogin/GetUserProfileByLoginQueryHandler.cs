using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Users.Queries.GetUserProfileByLogin
{
    public class GetUserProfileByLoginQueryHandler(
        IUserRepository userRepository,
        IPublicationRepository publicationRepository,
        IPublicationLikeRepositories publicationLikeRepositories,
        IFollowRepository followRepository) : IQueryHandler<GetUserProfileByLoginQuery, UserProfileDto>
    {
        public async Task<Result<UserProfileDto>> Handle(GetUserProfileByLoginQuery request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByLoginAsync(request.Login);
            if (user is null)
            {
                return Result.Failure<UserProfileDto>(new Error(
                    "User.NotFound",
                    $"User with login {request.Login} was not found"));
            }

            IReadOnlyList<Publication> publications = await publicationRepository.GetUserPublicationsByUserIdAsync(user.Id);
            IReadOnlyList<ShortPublicationDto>? publicationsDto = publications
                .Select(async p => new ShortPublicationDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    ImageData = p.ImageData,
                    CreationDateTime = p.CreationDateTime,
                    PublicationLikes = await publicationLikeRepositories.GetPublicationLikesListByPublicationIdAsync(p.Id),
                })
                .ToList() as IReadOnlyList<ShortPublicationDto>;

            IReadOnlyList<User> followers = await followRepository.GetFollowersUsersByUserIdAsync(user.Id);
            IReadOnlyList<User> following = await followRepository.GetFollowingUsersByUserIdAsync(user.Id);

            UserProfileDto userProfile = new()
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio,
                ProfileImageData = user.ProfileImageData,
                Publications = publicationsDto,
                FollowersCount = followers.Count,
                FollowingCount = following.Count,
            };

            return Result.Success(userProfile);
        }
    }
}
