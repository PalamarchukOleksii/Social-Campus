using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository(ApplicationDbContext context) : IRefreshTokenRepository
    {
        public async Task<RefreshToken> AddAsync(string token, int expiryTimeInDays)
        {
            RefreshToken refreshToken = new RefreshToken
            {
                Token = token,
                TokenExpiryTime = DateTime.UtcNow.AddDays(expiryTimeInDays)
            };

            await context.AddAsync(refreshToken);

            return refreshToken;
        }

        public async void DeleteByIdAsync(int id)
        {
            RefreshToken? refreshToken = await context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Id == id);
            if (refreshToken != null)
            {
                if (refreshToken.User != null)
                {
                    refreshToken.User.RefreshTokenId = 0;
                }

                context.RefreshTokens.Remove(refreshToken);
            }
        }

        public async Task<RefreshToken?> GetByIdAsync(int id)
        {
            return await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == id);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public void UpdateAsync(RefreshToken refreshToken, string newToken, int expiryTimeInDays)
        {
            refreshToken.Token = newToken;
            refreshToken.TokenExpiryTime = DateTime.UtcNow.AddDays(expiryTimeInDays);
        }
    }
}
