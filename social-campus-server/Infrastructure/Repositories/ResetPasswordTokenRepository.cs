using Domain.Abstractions.Repositories;
using Domain.Models.ResetPasswordTokenModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ResetPasswordTokenRepository(ApplicationDbContext context) : IResetPasswordTokenRepository
{
    private const int ExpirationInSeconds = 600;

    public async Task<ResetPasswordToken?> GetAsync(ResetPasswordTokenId id)
    {
        return await context.ResetPasswordTokens.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<ResetPasswordToken> AddAsync(UserId userPasswordToResetId, string tokenHash)
    {
        var existingToken = await context.ResetPasswordTokens
            .FirstOrDefaultAsync(t => t.UserId == userPasswordToResetId);

        if (existingToken != null)
            context.ResetPasswordTokens.Remove(existingToken);

        var token = new ResetPasswordToken(userPasswordToResetId, ExpirationInSeconds, tokenHash);
        await context.ResetPasswordTokens.AddAsync(token);

        return token;
    }

    public void Remove(ResetPasswordToken token)
    {
        context.ResetPasswordTokens.Remove(token);
    }
}