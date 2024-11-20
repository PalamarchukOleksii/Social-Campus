using Application.Abstractions.Messaging;
using Domain.Dtos;
using Domain.Models.UserModel;

namespace Application.Follows.Queries.GetFollowersList
{
    public record GetFollowersListQuery(UserId UserId) : IQuery<IReadOnlyList<UserFollowDto?>>;
}
