using Domain.Abstractions.Repositories;
using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context) : IRefreshTokenRepository
{
    public async Task<RefreshToken> AddAsync(string token, int expiryTimeInSeconds, UserId userId)
    {
        var expirationDateTime = DateTime.UtcNow.AddSeconds(expiryTimeInSeconds);
        RefreshToken newRefreshToken = new(token, expirationDateTime, userId);

        await context.AddAsync(newRefreshToken);

        return newRefreshToken;
    }

    public async Task DeleteByIdAsync(RefreshTokenId id)
    {
        var refreshToken = await context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Id == id);
        if (refreshToken != null)
        {
            refreshToken.User?.DropRefreshTokenIdOnRevoke();

            context.RefreshTokens.Remove(refreshToken);
        }
    }

    public async Task<RefreshToken?> GetByIdAsync(RefreshTokenId id)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == id);
    }

    public async Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
    }

    public void Update(RefreshToken refreshToken, string newToken, int expiryTimeInSeconds)
    {
        var expirationDateTime = DateTime.UtcNow.AddSeconds(expiryTimeInSeconds);
        refreshToken.UpdateTokenOnRefresh(newToken, expirationDateTime);
    }
}