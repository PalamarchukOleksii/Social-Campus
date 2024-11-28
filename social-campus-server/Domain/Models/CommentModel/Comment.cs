using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Domain.Models.CommentModel
{
    public class Comment
    {
        private Comment() { }

        public Comment(
            UserId creatorId,
            string description,
            PublicationId relatedPublicationId,
            CommentId replyToCommentId)
        {
            Id = new CommentId(Guid.NewGuid());
            CreatorId = creatorId;
            Description = description;
            RelatedPublicationId = relatedPublicationId;
            ReplyToCommentId = replyToCommentId;
            CreationDateTime = DateTime.UtcNow;
        }

        public Comment(
            UserId creatorId,
            string description,
            PublicationId relatedPublicationId)
        {
            Id = new CommentId(Guid.NewGuid());
            CreatorId = creatorId;
            Description = description;
            RelatedPublicationId = relatedPublicationId;
            CreationDateTime = DateTime.UtcNow;
        }

        public CommentId Id { get; private set; } = new CommentId(Guid.Empty);
        public string Description { get; private set; } = string.Empty;
        public DateTime CreationDateTime { get; private set; }
        public UserId CreatorId { get; private set; } = new UserId(Guid.Empty);
        public virtual User? Creator { get; }
        public PublicationId RelatedPublicationId { get; private set; } = new PublicationId(Guid.Empty);
        public virtual Publication? RelatedPublication { get; }
        public CommentId ReplyToCommentId { get; private set; } = new CommentId(Guid.Empty);
        public virtual Comment? ReplyToComment { get; }
        public virtual ICollection<Comment>? RepliedComments { get; }
    }
}
