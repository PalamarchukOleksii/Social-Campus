using Application.Abstractions.Messaging;

namespace Application.EmailVerificationTokens.Commands.Verify;

public record VerifyCommand(Guid Token, string Email) : ICommand;