﻿using Domain.Abstractions.Repositories;
using Domain.Models.CommentModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommentRepository(ApplicationDbContext context) : ICommentRepository
    {
        public async Task AddAsync(string description, UserId creatorId, PublicationId publicationId, CommentId? replyToCommentId = null)
        {
            Comment comment = replyToCommentId is null
                ? new Comment(creatorId, description, publicationId)
                : new Comment(creatorId, description, publicationId, replyToCommentId);

            await context.Comments.AddAsync(comment);
        }

        public async Task<Comment?> GetByIdAsync(CommentId commentId)
        {
            return await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<IReadOnlyList<Comment>> GetPublicationCommentsByPublicationIdAsync(PublicationId publicationId)
        {
            return await context.Comments
                .Where(c => c.RelatedPublicationId == publicationId)
                .Include(c => c.Creator)
                .Include(c => c.CommentLikes)
                .AsSplitQuery()
                .ToListAsync() as IReadOnlyList<Comment>;
        }

        public async Task<int> GetPublicationCommentsCountByPublicationIdAsync(PublicationId publicationId)
        {
            return (await context.Comments.Where(c => c.RelatedPublicationId == publicationId).ToListAsync()).Count;
        }

        public async Task<IReadOnlyList<Comment>> GetRepliedCommentsByCommentIdAsync(CommentId commentId)
        {
            return await context.Comments
                .Where(comment => comment.ReplyToCommentId == commentId)
                .Include(comment => comment.Creator)
                .Include(comment => comment.CommentLikes)
                .AsSplitQuery()
                .ToListAsync() as IReadOnlyList<Comment>;
        }

        public async Task<bool> IsExistByIdAsync(CommentId commentId)
        {
            return await context.Comments.AnyAsync(u => u.Id == commentId);
        }

        public void Update(Comment comment, string description)
        {
            comment.Update(description);

            context.Comments.Update(comment);
        }
    }
}
