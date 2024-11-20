using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Abstractions.Repositories;
using Domain.Models.UserModel;
using Domain.Shared;

namespace Application.Follows.Queries.GetFollowersList
{
    public class GetFollowersListQueryHandler(
        IFollowRepository followRepository) : IQueryHandler<GetFollowersListQuery, IReadOnlyList<UserFollowDto>>
    {
        public async Task<Result<IReadOnlyList<UserFollowDto>>> Handle(GetFollowersListQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<User> response = await followRepository.GetFollowersUsersByIdAsync(request.UserId);

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
