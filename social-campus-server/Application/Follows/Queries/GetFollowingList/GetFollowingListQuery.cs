using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.UserModel;

namespace Application.Follows.Queries.GetFollowingList
{
    public record GetFollowingListQuery(UserId UserId) : IQuery<IReadOnlyList<UserFollowDto>>;
}
