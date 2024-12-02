using Domain.Models.CommentModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories
{
    public interface ICommentRepository
    {
        public Task AddAsync(string description, UserId creatorId, PublicationId publicationId, CommentId? replyToCommentId = null);
        public Task<IReadOnlyList<Comment>> GetPublicationCommentsByPublicationIdAsync(PublicationId publicationId);
        public Task<int> GetPublicationCommentsCountByPublicationIdAsync(PublicationId publicationId);
        public Task<IReadOnlyList<Comment>> GetRepliedCommentsByCommentIdAsync(CommentId commentId);
        public void Update(Comment comment, string description);
        public Task<Comment?> GetByIdAsync(CommentId commentId);
        public Task<bool> IsExistByIdAsync(CommentId commentId);
    }
}
