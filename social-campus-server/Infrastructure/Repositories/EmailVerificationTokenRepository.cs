using Domain.Abstractions.Repositories;
using Domain.Models.EmailVerificationTokenModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmailVerificationTokenRepository(ApplicationDbContext context) : IEmailVerificationTokenRepository
{
    private const int ExpirationInSeconds = 86400;

    public async Task<EmailVerificationToken?> GetAsync(EmailVerificationTokenId id)
    {
        return await context.EmailVerificationTokens.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(UserId userEmailToVerifyId)
    {
        var existingToken = await context.EmailVerificationTokens
            .FirstOrDefaultAsync(t => t.UserId == userEmailToVerifyId);

        if (existingToken != null)
            context.EmailVerificationTokens.Remove(existingToken);

        var token = new EmailVerificationToken(userEmailToVerifyId, ExpirationInSeconds);
        await context.EmailVerificationTokens.AddAsync(token);
    }

    public void Remove(EmailVerificationToken token)
    {
        context.EmailVerificationTokens.Remove(token);
    }
}