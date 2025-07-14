using Domain.Models.CommentModel;
using Domain.Models.PublicationLikeModel;
using Domain.Models.PublicationTagModel;
using Domain.Models.UserModel;

namespace Domain.Models.PublicationModel;

public class Publication
{
    private Publication()
    {
    }

    public Publication(string description, UserId creatorId, string imageObjectKey)
    {
        Id = new PublicationId(Guid.NewGuid());
        Description = description;
        ImageObjectKey = imageObjectKey;
        CreatorId = creatorId;
        CreationDateTime = DateTime.UtcNow;
    }

    public PublicationId Id { get; private set; } = new(Guid.Empty);
    public string Description { get; private set; } = string.Empty;
    public string ImageObjectKey { get; private set; } = string.Empty;
    public DateTime CreationDateTime { get; private set; }
    public UserId CreatorId { get; private set; } = new(Guid.Empty);
    public virtual User? Creator { get; }
    public virtual ICollection<PublicationLike>? PublicationLikes { get; }
    public virtual ICollection<Comment>? Comments { get; }
    public virtual ICollection<PublicationTag>? PublicationTags { get; }

    public void Update(string description, string imageObjectKey)
    {
        Description = description;
        ImageObjectKey = imageObjectKey;
    }
}