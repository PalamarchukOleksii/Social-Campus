using Domain.Models.EmailVerificationTokenModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories;

public interface IEmailVerificationTokenRepository
{
    public Task<EmailVerificationToken?> GetAsync(EmailVerificationTokenId id);
    public Task<EmailVerificationToken> AddAsync(UserId userEmailToVerifyId);
    public void Remove(EmailVerificationToken token);
}