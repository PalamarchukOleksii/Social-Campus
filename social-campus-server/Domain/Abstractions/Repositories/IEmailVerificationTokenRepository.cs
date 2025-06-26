using Domain.Models.EmailVerificationTokenModel;

namespace Domain.Abstractions.Repositories;

public interface IEmailVerificationTokenRepository
{
    public Task<EmailVerificationToken?> GetByEmailAsync(string email);
    public Task<EmailVerificationToken> AddAsync(string email, string tokenHash);
    public void Remove(EmailVerificationToken token);
}