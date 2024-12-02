using Domain.Models.CommentModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories
{
    public interface ICommentLikeRepository
    {
        public Task AddAsync(UserId userId, CommentId commentId);
        public Task DeleteAsync(UserId userId, CommentId commentId);
        public Task<bool> IsLike(UserId userId, CommentId commentId);
    }
}
