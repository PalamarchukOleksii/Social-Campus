using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;
namespace Application.Users.Queries.GetUserProfileByLogin
{
    public class GetUserProfileByLoginHandler(
        IUserRepository userRepository,
        IPublicationLikeRepositories publicationLikeRepositories) : IQueryHandler<GetUserProfileByLoginQuery, UserProfileDto>
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

            IReadOnlyList<ShortPublicationDto> readOnlyPublicationsDto = user.Publications is null ? [] : await Task.WhenAll(user.Publications
                .Select(async p => new ShortPublicationDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    ImageData = p.ImageData,
                    CreationDateTime = p.CreationDateTime,
                    UserWhoLikedIds = (await publicationLikeRepositories
                        .GetPublicationLikesListByPublicationIdAsync(p.Id))
                        .Select(like => like.UserId)
                        .ToList() as IReadOnlyList<UserId>
                }));

            UserProfileDto userProfile = new()
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio,
                ProfileImageData = user.ProfileImageData,
                Publications = readOnlyPublicationsDto,
                FollowersCount = user.Followers?.Count ?? 0,
                FollowingCount = user.FollowedUsers?.Count ?? 0,
            };

            return Result.Success(userProfile);
        }
    }
}