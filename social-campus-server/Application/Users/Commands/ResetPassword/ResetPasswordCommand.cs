using Application.Abstractions.Messaging;
using Domain.Models.UserModel;

namespace Application.Users.Commands.ResetPassword;

public record ResetPasswordCommand(Guid ResetPasswordToken, UserId UserId, string NewPassword) : ICommand;