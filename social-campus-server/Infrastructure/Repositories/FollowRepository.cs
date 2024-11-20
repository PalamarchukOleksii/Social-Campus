using Domain.Abstractions.Repositories;
using Domain.Dtos;
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

        public async Task<IReadOnlyList<UserFollowDto?>> GetFollowersByIdAsync(UserId userId)
        {
            return await context.Follows
                .Where(f => f.FollowedUserId == userId)
                .Select(f => f.User == null ? null : new UserFollowDto
                {
                    Id = f.User.Id,
                    Login = f.User.Login,
                    FirstName = f.User.FirstName,
                    LastName = f.User.LastName,
                    Bio = f.User.Bio
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<UserFollowDto?>> GetFollowingByIdAsync(UserId userId)
        {
            return await context.Follows
                .Where(f => f.UserId == userId)
                .Select(f => f.FollowedUser == null ? null : new UserFollowDto
                {
                    Id = f.FollowedUser.Id,
                    Login = f.FollowedUser.Login,
                    FirstName = f.FollowedUser.FirstName,
                    LastName = f.FollowedUser.LastName,
                    Bio = f.FollowedUser.Bio
                })
                .ToListAsync();
        }

        public async Task<bool> IsFollowing(UserId userId, UserId followUserid)
        {
            return await context.Follows.AnyAsync(f => f.UserId == userId && f.FollowedUserId == followUserid);
        }
    }
}
