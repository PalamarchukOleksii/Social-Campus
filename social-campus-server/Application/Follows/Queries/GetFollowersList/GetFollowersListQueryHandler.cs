using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Follows.Queries.GetFollowersList
{
    public class GetFollowersListQueryHandler(
        IFollowRepository followRepository,
        IUserRepository userRepository) : IQueryHandler<GetFollowersListQuery, IReadOnlyList<UserDto>>
    {
        public async Task<Result<IReadOnlyList<UserDto>>> Handle(GetFollowersListQuery request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByLoginAsync(request.Login);
            if (user is null)
            {
                return Result.Failure<IReadOnlyList<UserDto>>(new Error(
                    "User.NotFound",
                    $"User with login {request.Login} was not found"));
            }

            IReadOnlyList<User> response = await followRepository.GetFollowersUsersByUserIdAsync(user.Id);

            List<UserDto> followersDto = new();

            foreach (var followingUser in response)
            {
                var userFollowers = await followRepository.GetFollowersUsersByUserIdAsync(followingUser.Id);

                followersDto.Add(new UserDto
                {
                    Id = followingUser.Id,
                    Login = followingUser.Login,
                    FirstName = followingUser.FirstName,
                    LastName = followingUser.LastName,
                    Bio = followingUser.Bio,
                    ProfileImageData = followingUser.ProfileImageData,
                    FollowersIds = userFollowers
                        .Select(f => f.Id)
                        .ToList() as IReadOnlyList<UserId>
                });
            }

            return Result.Success(followersDto as IReadOnlyList<UserDto>);
        }
    }
}
