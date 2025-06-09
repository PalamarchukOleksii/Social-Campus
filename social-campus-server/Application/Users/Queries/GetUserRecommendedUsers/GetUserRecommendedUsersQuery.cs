using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.UserModel;

namespace Application.Users.Queries.GetUserRecommendedUsers;

public record GetUserRecommendedUsersQuery(UserId UserId) : IQuery<IReadOnlyList<UserDto>>;