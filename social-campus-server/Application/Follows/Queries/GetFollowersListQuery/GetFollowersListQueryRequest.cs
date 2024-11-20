using Domain.Dtos;
using Domain.Models.UserModel;
using MediatR;

namespace Application.Follows.Queries.GetFollowersListQuery
{
    public record GetFollowersListQueryRequest(UserId UserId) : IRequest<IReadOnlyList<UserFollowDto?>>;
}
