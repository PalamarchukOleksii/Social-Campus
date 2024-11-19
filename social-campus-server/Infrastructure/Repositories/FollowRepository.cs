using Domain.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FollowRepository(ApplicationDbContext context) : IFollowRepository
    {
        public async void AddAsync(int userId, int followUserId)
        {
            Follow newFollow = new()
            {
                UserId = userId,
                FollowedUserId = followUserId,
            };

            await context.Follows.AddAsync(newFollow);
        }

        public async void DeleteAsync(int userId, int followUserId)
        {
            Follow? follow = await context.Follows.FirstOrDefaultAsync(f => f.UserId == userId && f.FollowedUserId == followUserId);
            if (follow != null)
            {
                context.Follows.Remove(follow);
            }
        }

        public async Task<List<User?>> GetFollowersByIdAsync(int userId)
        {
            return await context.Follows
                .Where(f => f.FollowedUserId == userId)
                .Select(f => f.User)
                .ToListAsync();
        }

        public async Task<List<User?>> GetFollowingByIdAsync(int userId)
        {
            return await context.Follows
                .Where(f => f.UserId == userId)
                .Select(f => f.FollowedUser)
                .ToListAsync();
        }

        public async Task<bool> IsFollowing(int userId, int followUserid)
        {
            return await context.Follows.AnyAsync(f => f.UserId == userId && f.FollowedUserId == followUserid);
        }
    }
}
