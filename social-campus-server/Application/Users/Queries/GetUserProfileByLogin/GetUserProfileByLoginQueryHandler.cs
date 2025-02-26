using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Domain.Shared;
namespace Application.Users.Queries.GetUserProfileByLogin
{
    public class GetUserProfileByLoginHandler(
        IUserRepository userRepository,
        IPublicationLikeRepositories publicationLikeRepositories,
        IPublicationRepository publicationRepository,
        IFollowRepository followRepository,
        ICommentRepository commentRepository) : IQueryHandler<GetUserProfileByLoginQuery, UserProfileDto>
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
            IReadOnlyList<User> following = await followRepository.GetFollowingUsersByUserIdAsync(user.Id);
            IReadOnlyList<User> followers = await followRepository.GetFollowersUsersByUserIdAsync(user.Id);

            List<ShortPublicationDto> readOnlyPublicationsDto = [];
            foreach (var p in publications)
            {
                var publicationDto = new ShortPublicationDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    ImageData = p.ImageData,
                    CreationDateTime = p.CreationDateTime,
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
                    UserWhoLikedIds = (await publicationLikeRepositories
                        .GetPublicationLikesListByPublicationIdAsync(p.Id))
                        .Select(like => like.UserId)
                        .ToList() as IReadOnlyList<UserId>,
                    CommentsCount = await commentRepository.GetPublicationCommentsCountByPublicationIdAsync(p.Id)
                };

                readOnlyPublicationsDto.Add(publicationDto);
            }


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
                FollowersCount = followers.Count,
                FollowingCount = following.Count,
                FollowersIds = followers
                        .Select(f => f.Id)
                        .ToList() as IReadOnlyList<UserId>
            };

            return Result.Success(userProfile);
        }
    }
}