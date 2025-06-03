using Domain.Abstractions.Repositories;
using Domain.Models.FollowModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FollowRepository(ApplicationDbContext context) : IFollowRepository
{
    public async Task AddAsync(UserId userId, UserId followUserId)
    {
        Follow newFollow = new(userId, followUserId);

        await context.Follows.AddAsync(newFollow);
    }

    public async Task DeleteAsync(UserId userId, UserId followUserId)
    {
        var follow =
            await context.Follows.FirstOrDefaultAsync(f => f.UserId == userId && f.FollowedUserId == followUserId);
        if (follow != null) context.Follows.Remove(follow);
    }

    public async Task<IReadOnlyList<User>> GetFollowersUsersByUserIdAsync(UserId userId)
    {
        return await context.Follows
            .Where(f => f.FollowedUserId == userId)
            .Select(f => f.User)
            .Where(user => user != null)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<User>> GetFollowingUsersByUserIdAsync(UserId userId)
    {
        return await context.Follows
            .Where(f => f.UserId == userId)
            .Select(f => f.FollowedUser)
            .Where(user => user != null)
            .ToListAsync();
    }

    public async Task<bool> IsFollowing(UserId userId, UserId followUserid)
    {
        return await context.Follows.AnyAsync(f => f.UserId == userId && f.FollowedUserId == followUserid);
    }
}