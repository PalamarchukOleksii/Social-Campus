using Domain.Dtos;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories
{
    public interface IFollowRepository
    {
        public Task AddAsync(UserId userId, UserId followUserId);
        public Task DeleteAsync(UserId userId, UserId followUserId);
        public Task<IReadOnlyList<UserFollowDto?>> GetFollowingByIdAsync(UserId userId);
        public Task<IReadOnlyList<UserFollowDto?>> GetFollowersByIdAsync(UserId userId);
        public Task<bool> IsFollowing(UserId userId, UserId followUserid);
    }
}
