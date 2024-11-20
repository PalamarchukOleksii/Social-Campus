using Domain.Abstractions.Repositories;
using Domain.Models.FollowModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FollowRepository(ApplicationDbContext context) : IFollowRepository
    {
        public async Task AddAsync(UserId userId, UserId followUserId)
        {
            Follow newFollow = new(userId, followUserId);

            await context.Follows.AddAsync(newFollow);
        }

        public async Task DeleteAsync(UserId userId, UserId followUserId)
        {
            Follow? follow = await context.Follows.FirstOrDefaultAsync(f => f.UserId == userId && f.FollowedUserId == followUserId);
            if (follow != null)
            {
                context.Follows.Remove(follow);
            }
        }

        public async Task<IReadOnlyList<User>> GetFollowersUsersByIdAsync(UserId userId)
        {
            return await context.Follows
                .Where(f => f.FollowedUserId == userId)
                .Select(f => f.User)
                .Where(user => user != null)
                .ToListAsync() as IReadOnlyList<User>;
        }

        public async Task<IReadOnlyList<User>> GetFollowingUsersByIdAsync(UserId userId)
        {
            return await context.Follows
                .Where(f => f.UserId == userId)
                .Select(f => f.FollowedUser)
                .Where(user => user != null)
                .ToListAsync() as IReadOnlyList<User>;
        }

        public async Task<bool> IsFollowing(UserId userId, UserId followUserid)
        {
            return await context.Follows.AnyAsync(f => f.UserId == userId && f.FollowedUserId == followUserid);
        }
    }
}
