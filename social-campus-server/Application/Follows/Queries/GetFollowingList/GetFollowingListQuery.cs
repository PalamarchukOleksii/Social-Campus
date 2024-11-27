using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.Follows.Queries.GetFollowingList
{
    public record GetFollowingListQuery(string Login) : IQuery<IReadOnlyList<UserFollowDto>>;
}
