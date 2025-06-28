using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.UserModel;

namespace Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    UserId CallerId,
    UserId UserId,
    string Login,
    string FirstName,
    string LastName,
    string Bio,
    FileUploadDto? ProfileImage) : ICommand;