using Domain.Models.CommentModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Presentation.Endpoints.Comments.ReplyToComment;

public class ReplyToCommentRequest
{
    public PublicationId PublicationId { get; set; } = new(Guid.Empty);
    public CommentId ReplyToCommentId { get; set; } = new(Guid.Empty);
    public string Description { get; set; } = string.Empty;
    public UserId CreatorId { get; set; } = new(Guid.Empty);
}