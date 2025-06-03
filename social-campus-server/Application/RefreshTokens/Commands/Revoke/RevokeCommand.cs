using Application.Abstractions.Messaging;

namespace Application.RefreshTokens.Commands.Revoke;

public record RevokeCommand(string RefreshToken) : ICommand;