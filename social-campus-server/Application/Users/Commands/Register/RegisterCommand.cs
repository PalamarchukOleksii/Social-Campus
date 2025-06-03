using Application.Abstractions.Messaging;

namespace Application.Users.Commands.Register;

public record RegisterCommand(
    string Login,
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand;