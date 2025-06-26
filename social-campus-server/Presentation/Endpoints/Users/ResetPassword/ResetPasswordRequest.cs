using Domain.Models.UserModel;

namespace Presentation.Endpoints.Users.ResetPassword;

public class ResetPasswordRequest
{
    public Guid ResetPasswordToken { get; set; } = Guid.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    public string NewPassword { get; set; } = string.Empty;
}