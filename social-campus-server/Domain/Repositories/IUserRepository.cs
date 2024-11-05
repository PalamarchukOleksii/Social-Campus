using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> IsEmailUniqueAsync(string email);
        public Task<bool> IsLoginUniqueAsync(string login);
        public void AddAsync(User user);
        public Task<User?> GetByEmailAsync(string email);
        public Task<User?> GetByRefreshTokenIdAsync(int refreshTokenId);
    }
}
