using Application.Abstractions.Messaging;
using Domain.Models.UserModel;

namespace Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(UserId UserId, UserId CallerId) : ICommand;