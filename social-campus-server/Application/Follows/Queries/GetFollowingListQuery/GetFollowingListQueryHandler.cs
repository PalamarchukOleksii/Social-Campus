using Domain.Abstractions.Repositories;
using Domain.Dtos;
using MediatR;

namespace Application.Follows.Queries.GetFollowingListQuery
{
    public class GetFollowingListQueryHandler(
        IFollowRepository followRepository) : IRequestHandler<GetFollowingListQueryRequest, IReadOnlyList<UserFollowDto?>>
    {
        public async Task<IReadOnlyList<UserFollowDto?>> Handle(GetFollowingListQueryRequest request, CancellationToken cancellationToken)
        {
            return await followRepository.GetFollowingByIdAsync(request.UserId);
        }
    }
}
