using Application.Abstractions.Messaging;
using Domain.Abstractions.Repositories;
using Domain.Dtos;
using Domain.Shared;

namespace Application.Follows.Queries.GetFollowingList
{
    public class GetFollowingListQueryHandler(
        IFollowRepository followRepository) : IQueryHandler<GetFollowingListQuery, IReadOnlyList<UserFollowDto?>>
    {
        public async Task<Result<IReadOnlyList<UserFollowDto?>>> Handle(GetFollowingListQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<UserFollowDto?> response = await followRepository.GetFollowingByIdAsync(request.UserId);

            return Result.Success(response);
        }
    }
}
