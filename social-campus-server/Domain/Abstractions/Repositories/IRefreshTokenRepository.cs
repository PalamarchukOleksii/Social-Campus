using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories
{
    public interface IRefreshTokenRepository
    {
        public Task<RefreshToken?> GetByIdAsync(RefreshTokenId id);
        public Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken);
        public Task<RefreshToken> AddAsync(string token, int expiryTimeInDays, UserId userId);
        public void Update(RefreshToken refreshToken, string newToken, int expiryTimeInDays);
        public Task DeleteByIdAsync(RefreshTokenId id);
    }
}
