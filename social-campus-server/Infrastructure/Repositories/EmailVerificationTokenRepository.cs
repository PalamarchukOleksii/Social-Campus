using Domain.Abstractions.Repositories;
using Domain.Models.EmailVerificationTokenModel;
using Infrastructure.Data;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories;

public class EmailVerificationTokenRepository(
    ApplicationDbContext context,
    IOptions<EmailVerificationTokenOptions> options) : IEmailVerificationTokenRepository
{
    private readonly int _expirationInSeconds = options.Value.ExpirationInSeconds;

    public async Task<EmailVerificationToken?> GetByEmailAsync(string email)
    {
        return await context.EmailVerificationTokens.FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<EmailVerificationToken> AddAsync(string email, string tokenHash)
    {
        var existingToken = await context.EmailVerificationTokens
            .FirstOrDefaultAsync(t => t.Email == email);

        if (existingToken != null)
            context.EmailVerificationTokens.Remove(existingToken);

        var token = new EmailVerificationToken(email, tokenHash, _expirationInSeconds);
        await context.EmailVerificationTokens.AddAsync(token);

        return token;
    }

    public void Remove(EmailVerificationToken token)
    {
        context.EmailVerificationTokens.Remove(token);
    }
}