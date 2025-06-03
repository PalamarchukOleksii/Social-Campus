using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.Users.Queries.GetUserByLogin;

public record GetUserByLoginQuery(string Login) : IQuery<UserDto>;