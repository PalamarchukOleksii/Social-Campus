using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.Users.Queries.SearchUsers;

public record SearchUsersQuery(string SearchTerm, int Page, int Count) : IQuery<IReadOnlyList<UserDto>>;