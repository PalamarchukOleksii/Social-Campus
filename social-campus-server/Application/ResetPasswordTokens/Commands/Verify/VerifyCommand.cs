using Application.Abstractions.Messaging;
using Domain.Models.UserModel;

namespace Application.ResetPasswordTokens.Commands.Verify;

public record VerifyCommand(Guid Token, UserId UserId) : ICommand;