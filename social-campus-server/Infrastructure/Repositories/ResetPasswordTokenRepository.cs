using Domain.Abstractions.Repositories;
using Domain.Models.ResetPasswordTokenModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories;

public class ResetPasswordTokenRepository(ApplicationDbContext context, IOptions<ResetPasswordTokenOptions> options)
    : IResetPasswordTokenRepository
{
    private readonly int _expirationInSeconds = options.Value.ExpirationInSeconds;

    public async Task<ResetPasswordToken?> GetByUserIdAsync(UserId userId)
    {
        return await context.ResetPasswordTokens
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.CreatedOnUtc)
            .FirstOrDefaultAsync();
    }

    public async Task<ResetPasswordToken> AddAsync(UserId userPasswordToResetId, string tokenHash)
    {
        var existingToken = await context.ResetPasswordTokens
            .FirstOrDefaultAsync(t => t.UserId == userPasswordToResetId);

        if (existingToken != null)
            context.ResetPasswordTokens.Remove(existingToken);

        var token = new ResetPasswordToken(userPasswordToResetId, _expirationInSeconds, tokenHash);
        await context.ResetPasswordTokens.AddAsync(token);

        return token;
    }

    public void Remove(ResetPasswordToken token)
    {
        context.ResetPasswordTokens.Remove(token);
    }
}