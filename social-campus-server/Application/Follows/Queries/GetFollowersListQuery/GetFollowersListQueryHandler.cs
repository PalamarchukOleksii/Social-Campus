using Domain.Abstractions.Repositories;
using Domain.Dtos;
using MediatR;

namespace Application.Follows.Queries.GetFollowersListQuery
{
    public class GetFollowersListQueryHandler(
        IFollowRepository followRepository) : IRequestHandler<GetFollowersListQueryRequest, IReadOnlyList<UserFollowDto?>>
    {
        public async Task<IReadOnlyList<UserFollowDto?>> Handle(GetFollowersListQueryRequest request, CancellationToken cancellationToken)
        {
            return await followRepository.GetFollowersByIdAsync(request.UserId);
        }
    }
}
