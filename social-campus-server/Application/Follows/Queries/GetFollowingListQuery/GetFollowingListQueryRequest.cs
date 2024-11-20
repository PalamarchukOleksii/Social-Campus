using Domain.Dtos;
using Domain.Models.UserModel;
using MediatR;

namespace Application.Follows.Queries.GetFollowingListQuery
{
    public record GetFollowingListQueryRequest(UserId UserId) : IRequest<IReadOnlyList<UserFollowDto?>>;
}
