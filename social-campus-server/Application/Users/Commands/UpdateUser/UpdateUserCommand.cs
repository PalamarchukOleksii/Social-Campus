using Application.Abstractions.Messaging;
using Domain.Models.UserModel;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    UserId CallerId,
    UserId UserId,
    string Login,
    string FirstName,
    string LastName,
    string Bio,
    IFormFile? ProfileImage) : ICommand;