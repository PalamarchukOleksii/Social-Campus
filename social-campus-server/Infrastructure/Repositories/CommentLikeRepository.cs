using Domain.Abstractions.Repositories;
using Domain.Models.CommentLikeModel;
using Domain.Models.CommentModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommentLikeRepository(
        ApplicationDbContext context) : ICommentLikeRepository
    {
        public async Task AddAsync(UserId userId, CommentId commentId)
        {
            CommentLike newCommentLike = new(userId, commentId);

            await context.CommentLikes.AddAsync(newCommentLike);
        }

        public async Task DeleteAsync(UserId userId, CommentId commentId)
        {
            CommentLike? commentLike = await context.CommentLikes
                .FirstOrDefaultAsync(pl => pl.CommentId == commentId && pl.UserId == userId);
            if (commentLike != null)
            {
                context.CommentLikes.Remove(commentLike);
            }
        }

        public async Task<bool> IsLike(UserId userId, CommentId commentId)
        {
            return await context.CommentLikes.AnyAsync(pl => pl.CommentId == commentId && pl.UserId == userId);
        }
    }
}
