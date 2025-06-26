using Application.Abstractions.Messaging;

namespace Application.EmailVerificationTokens.Commands.Generate;

public record GenerateCommand(string Email) : ICommand;