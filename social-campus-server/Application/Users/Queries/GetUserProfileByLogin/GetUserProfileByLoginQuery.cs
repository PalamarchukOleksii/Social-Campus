using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.Users.Queries.GetUserProfileByLogin
{
    public record GetUserProfileByLoginQuery(string Login) : IQuery<UserProfileDto>;
}
