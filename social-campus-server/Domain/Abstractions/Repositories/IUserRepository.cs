using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories;

public interface IUserRepository
{
    public Task<bool> IsEmailUniqueAsync(string email);
    public Task<bool> IsLoginUniqueAsync(string login);
    public Task AddAsync(string login, string passwordHash, string email, string firstName, string lastName);
    public Task<User?> GetByEmailAsync(string email);
    public Task<User?> GetByLoginAsync(string login);
    public Task<User?> GetByIdAsync(UserId id);
    public Task<User?> GetByRefreshTokenIdAsync(RefreshTokenId refreshTokenId);
    public Task<bool> IsExistByIdAsync(UserId userId);

    public void Update(User user, string login, string email, string firstName, string lastName, string bio,
        string profileImageData);
}