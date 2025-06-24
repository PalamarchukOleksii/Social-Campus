using Domain.Models.EmailVerificationTokenModel;

namespace Application.Abstractions.Email;

public interface IEmailVerificationLinkFactory
{
    public string? Create(EmailVerificationTokenId emailVerificationTokenId);
}