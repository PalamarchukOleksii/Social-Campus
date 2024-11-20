using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Dtos;
using Domain.Shared;

namespace Application.Follows.Queries.GetFollowersList
{
    public class GetFollowersListQueryHandler(
        IFollowRepository followRepository) : IQueryHandler<GetFollowersListQuery, IReadOnlyList<UserFollowDto?>>
    {
        public async Task<Result<IReadOnlyList<UserFollowDto?>>> Handle(GetFollowersListQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<UserFollowDto?> response = await followRepository.GetFollowersByIdAsync(request.UserId);

            return Result.Success(response);
        }
    }
}
