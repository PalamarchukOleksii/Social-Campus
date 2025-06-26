using Domain.Abstractions.Repositories;
using Domain.Models.EmailVerificationTokenModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmailVerificationTokenRepository(ApplicationDbContext context) : IEmailVerificationTokenRepository
{
    private const int ExpirationInSeconds = 600;

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

        var token = new EmailVerificationToken(email, tokenHash, ExpirationInSeconds);
        await context.EmailVerificationTokens.AddAsync(token);

        return token;
    }

    public void Remove(EmailVerificationToken token)
    {
        context.EmailVerificationTokens.Remove(token);
    }
}