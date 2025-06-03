using Domain.Models.CommentModel;
using Domain.Models.UserModel;

namespace Domain.Models.CommentLikeModel;

public class CommentLike
{
    private CommentLike()
    {
    }

    public CommentLike(UserId userId, CommentId commentId)
    {
        Id = new CommentLikeId(Guid.NewGuid());
        UserId = userId;
        CommentId = commentId;
    }

    public CommentLikeId Id { get; private set; } = new(Guid.Empty);
    public UserId UserId { get; private set; } = new(Guid.Empty);
    public virtual User? User { get; }
    public CommentId CommentId { get; private set; } = new(Guid.Empty);
    public virtual Comment? Comment { get; }
}