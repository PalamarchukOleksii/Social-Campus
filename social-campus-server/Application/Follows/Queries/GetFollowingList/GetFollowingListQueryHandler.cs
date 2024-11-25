using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Follows.Queries.GetFollowingList
{
    public class GetFollowingListQueryHandler(
        IFollowRepository followRepository,
        IUserRepository userRepository) : IQueryHandler<GetFollowingListQuery, IReadOnlyList<UserFollowDto>>
    {
        public async Task<Result<IReadOnlyList<UserFollowDto>>> Handle(GetFollowingListQuery request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                return Result.Failure<IReadOnlyList<UserFollowDto>>(new Error(
                    "User.NotFound",
                    $"User with UserId {request.UserId} was not found"));
            }

            IReadOnlyList<User> response = await followRepository.GetFollowingUsersByIdAsync(request.UserId);

            IReadOnlyList<UserFollowDto> followingDto = response
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

            return Result.Success(followingDto);
        }
    }
}
