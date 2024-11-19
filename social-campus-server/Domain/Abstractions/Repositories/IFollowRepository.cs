using Domain.Entities;

namespace Domain.Abstractions.Repositories
{
    public interface IFollowRepository
    {
        public void AddAsync(int userId, int followUserId);
        public void DeleteAsync(int userId, int followUserId);
        public Task<List<User?>> GetFollowingByIdAsync(int userId);
        public Task<List<User?>> GetFollowersByIdAsync(int userId);
        public Task<bool> IsFollowing(int userId, int followUserid);
    }
}
