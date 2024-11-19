using Domain.Entities;

namespace Domain.Abstractions.Repositories
{
    public interface IRefreshTokenRepository
    {
        public Task<RefreshToken?> GetByIdAsync(int id);
        public Task<RefreshToken?> GetByTokenAsync(string token);
        public Task<RefreshToken> AddAsync(string token, int expiryTimeInDays, int userId);
        public void DeleteByIdAsync(int id);
        public void UpdateAsync(RefreshToken refreshToken, string newToken, int expiryTimeInDays);
    }
}
