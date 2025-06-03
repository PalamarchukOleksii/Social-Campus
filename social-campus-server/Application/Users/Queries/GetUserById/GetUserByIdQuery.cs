using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.UserModel;

namespace Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(UserId UserId) : IQuery<UserDto>;