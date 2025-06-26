using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories;

public interface IUserRepository
{
    public Task<bool> IsEmailUniqueAsync(string email);
    public Task<bool> IsLoginUniqueAsync(string login);
    public Task<User> AddAsync(string login, string passwordHash, string email, string firstName, string lastName);
    public Task<User?> GetByEmailAsync(string email);
    public Task<User?> GetByLoginAsync(string login);
    public Task<User?> GetByIdAsync(UserId id);
    public Task<User?> GetByRefreshTokenIdAsync(RefreshTokenId refreshTokenId);
    public Task<bool> IsExistByIdAsync(UserId userId);
    public Task<IReadOnlyList<User>> GetRandomUsersAsync(int count);

    public void Update(User user, string login, string firstName, string lastName, string bio,
        string profileImageUrl);

    public Task<IReadOnlyList<User>> SearchAsync(string searchTerm, int page, int count);
    public Task DeleteAsync(User user);
    public void UpdatePassword(User user, string passwordHash);
}