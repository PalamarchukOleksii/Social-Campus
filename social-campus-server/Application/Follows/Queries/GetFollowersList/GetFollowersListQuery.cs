using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.Follows.Queries.GetFollowersList
{
    public record GetFollowersListQuery(string Login) : IQuery<IReadOnlyList<UserFollowDto>>;
}
