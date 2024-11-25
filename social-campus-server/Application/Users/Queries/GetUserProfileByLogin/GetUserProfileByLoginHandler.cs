using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Users.Queries.GetUserProfileByLogin
{
    public class GetUserProfileByLoginHandler(
        IUserRepository userRepository) : IQueryHandler<GetUserProfileByLoginQuery, UserProfileDto>
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

            IReadOnlyList<ShortPublicationDto>? publicationsDto = user.Publications?
                .Select(p => new ShortPublicationDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    ImageData = p.ImageData,
                    CreationDateTime = p.CreationDateTime
                })
                .ToList() as IReadOnlyList<ShortPublicationDto>;

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
                FollowersCount = user.Followers?.Count ?? 0,
                FollowingCount = user.FollowedUsers?.Count ?? 0,
            };

            return Result.Success(userProfile);
        }
    }
}
