using Domain.Abstractions.Repositories;
using Domain.Models.RefreshTokenModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(
    ApplicationDbContext context,
    IPublicationRepository publicationRepository,
    ICommentRepository commentRepository) : IUserRepository
{
    public async Task<User> AddAsync(string login, string passwordHash, string email, string firstName, string lastName)
    {
        User newUser = new(login, passwordHash, email, firstName, lastName);
        await context.Users.AddAsync(newUser);

        return newUser;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(UserId id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<User?> GetByRefreshTokenIdAsync(RefreshTokenId refreshTokenId)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.RefreshTokenId == refreshTokenId);
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return !await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> IsExistByIdAsync(UserId userId)
    {
        return await context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<IReadOnlyList<User>> GetRandomUsersAsync(int count)
    {
        return await context.Users
            .OrderBy(u => Guid.NewGuid())
            .Take(count)
            .ToListAsync();
    }

    public async Task<bool> IsLoginUniqueAsync(string login)
    {
        return !await context.Users.AnyAsync(u => u.Login == login);
    }

    public void Update(User user, string login, string email, string firstName, string lastName, string bio,
        string profileImageUrl)
    {
        user.Update(login, email, firstName, lastName, bio, profileImageUrl);

        context.Update(user);
    }

    public async Task<IReadOnlyList<User>> SearchAsync(string searchTerm, int page, int count)
    {
        searchTerm = $"%{searchTerm.ToLower().Trim()}%";

        var users = await context.Users
            .Where(u =>
                EF.Functions.Like(u.Login.ToLower(), searchTerm) ||
                EF.Functions.Like(u.FirstName.ToLower(), searchTerm) ||
                EF.Functions.Like(u.LastName.ToLower(), searchTerm))
            .Include(u => u.Followers)
            .Skip((page - 1) * count)
            .Take(count)
            .ToListAsync();

        return users
            .OrderByDescending(u => u.Followers?.Count ?? 0)
            .ToList();
    }

    public async Task DeleteAsync(User user)
    {
        var commentLikes = context.CommentLikes.Where(cl => cl.UserId == user.Id);
        context.CommentLikes.RemoveRange(commentLikes);

        var publicationLikes = context.PublicationLikes.Where(pl => pl.UserId == user.Id);
        context.PublicationLikes.RemoveRange(publicationLikes);

        var comments = await context.Comments.Where(c => c.CreatorId == user.Id).ToListAsync();
        foreach (var comment in comments) await commentRepository.DeleteAsync(comment);

        var publications = await context.Publications.Where(p => p.CreatorId == user.Id).ToListAsync();
        foreach (var publication in publications) await publicationRepository.DeleteAsync(publication);

        var follows = context.Follows.Where(f => f.UserId == user.Id || f.FollowedUserId == user.Id);
        context.Follows.RemoveRange(follows);

        var passwordTokens = context.ResetPasswordTokens.Where(t => t.UserId == user.Id);
        context.ResetPasswordTokens.RemoveRange(passwordTokens);

        var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == user.Id);
        if (refreshToken != null)
            context.RefreshTokens.Remove(refreshToken);

        context.Users.Remove(user);
    }

    public void UpdatePassword(User user, string passwordHash)
    {
        user.UpdatePasswordHash(passwordHash);

        context.Update(user);
    }
}