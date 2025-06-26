using Domain.Models.EmailVerificationTokenModel;
using Domain.Models.ResetPasswordTokenModel;
using Domain.Models.UserModel;

namespace Application.Abstractions.Email;

public interface IEmailLinkFactory
{
    public string? CreateEmailVerificationLink(EmailVerificationTokenId emailVerificationTokenId);
    public string? CreateResetPasswordLink(Guid generatedToken, UserId userId);
}