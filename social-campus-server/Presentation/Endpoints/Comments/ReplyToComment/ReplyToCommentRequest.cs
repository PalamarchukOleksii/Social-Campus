using Domain.Models.CommentModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Presentation.Endpoints.Comments.ReplyToComment
{
    public class ReplyToCommentRequest
    {
        public PublicationId PublicationId { get; set; } = new PublicationId(Guid.Empty);
        public CommentId ReplyToCommentId { get; set; } = new CommentId(Guid.Empty);
        public string Description { get; set; } = string.Empty;
        public UserId CreatorId { get; set; } = new UserId(Guid.Empty);
    }
}
