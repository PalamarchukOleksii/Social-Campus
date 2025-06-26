using Domain.Models.UserModel;

namespace Application.Abstractions.Email;

public interface IEmailLinkFactory
{
    public string? CreateEmailVerificationLink(Guid generatedToken, string userEmail);
    public string? CreateResetPasswordLink(Guid generatedToken, UserId userId);
}