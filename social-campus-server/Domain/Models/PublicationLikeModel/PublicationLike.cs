using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Domain.Models.PublicationLikeModel;

public class PublicationLike
{
    private PublicationLike()
    {
    }

    public PublicationLike(UserId userId, PublicationId publicationId)
    {
        Id = new PublicationLikeId(Guid.NewGuid());
        UserId = userId;
        PublicationId = publicationId;
    }

    public PublicationLikeId Id { get; private set; } = new(Guid.Empty);
    public UserId UserId { get; private set; } = new(Guid.Empty);
    public virtual User? User { get; }
    public PublicationId PublicationId { get; private set; } = new(Guid.Empty);
    public virtual Publication? Publication { get; }
}