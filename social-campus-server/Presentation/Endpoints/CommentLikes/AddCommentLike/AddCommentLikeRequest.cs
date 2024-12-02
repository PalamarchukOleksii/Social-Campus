using Domain.Models.CommentModel;
using Domain.Models.UserModel;

namespace Presentation.Endpoints.CommentLikes.AddCommentLike
{
    public class AddCommentLikeRequest
    {
        public UserId UserId { get; set; } = new UserId(Guid.Empty);
        public CommentId CommentId { get; set; } = new CommentId(Guid.Empty);
    }
}
