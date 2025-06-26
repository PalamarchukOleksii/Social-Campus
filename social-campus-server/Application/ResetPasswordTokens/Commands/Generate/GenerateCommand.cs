using Application.Abstractions.Messaging;

namespace Application.ResetPasswordTokens.Commands.Generate;

public record GenerateCommand(string Email) : ICommand;