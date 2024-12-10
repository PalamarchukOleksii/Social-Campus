using Domain.Models.CommentModel;
using Domain.Models.UserModel;

namespace Presentation.Endpoints.Comments.UpdateComment
{
    public class UpdateCommentRequest
    {
        public UserId CallerId { get; set; } = new UserId(Guid.Empty);
        public CommentId CommentId { get; set; } = new CommentId(Guid.Empty);
        public string Description { get; set; } = string.Empty;
    }
}
