using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Follows.Queries.GetFollowersList
{
    public class GetFollowersListQueryHandler(
        IFollowRepository followRepository,
        IUserRepository userRepository) : IQueryHandler<GetFollowersListQuery, IReadOnlyList<UserFollowDto>>
    {
        public async Task<Result<IReadOnlyList<UserFollowDto>>> Handle(GetFollowersListQuery request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByLoginAsync(request.Login);
            if (user is null)
            {
                return Result.Failure<IReadOnlyList<UserFollowDto>>(new Error(
                    "User.NotFound",
                    $"User with login {request.Login} was not found"));
            }

            IReadOnlyList<User> response = await followRepository.GetFollowersUsersByUserIdAsync(user.Id);

            IReadOnlyList<UserFollowDto> followersDto = response
                .Select(user => new UserFollowDto
                {
                    Id = user.Id,
                    Login = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Bio = user.Bio,
                    ProfileImageData = user.ProfileImageData,
                })
                .ToList();

            return Result.Success(followersDto);
        }
    }
}
