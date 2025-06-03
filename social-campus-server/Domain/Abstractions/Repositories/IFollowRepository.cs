using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories;

public interface IFollowRepository
{
    public Task AddAsync(UserId userId, UserId followUserId);
    public Task DeleteAsync(UserId userId, UserId followUserId);
    public Task<IReadOnlyList<User>> GetFollowingUsersByUserIdAsync(UserId userId);
    public Task<IReadOnlyList<User>> GetFollowersUsersByUserIdAsync(UserId userId);
    public Task<bool> IsFollowing(UserId userId, UserId followUserid);
}