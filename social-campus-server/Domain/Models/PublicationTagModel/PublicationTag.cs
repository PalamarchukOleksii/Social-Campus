using Domain.Models.PublicationModel;
using Domain.Models.TagModel;

namespace Domain.Models.PublicationTagModel;

public class PublicationTag
{
    private PublicationTag()
    {
    }

    public PublicationTag(TagId tagId, PublicationId publicationId)
    {
        Id = new PublicationTagId(Guid.NewGuid());
        TagId = tagId;
        PublicationId = publicationId;
    }

    public PublicationTagId Id { get; private set; } = new(Guid.Empty);
    public TagId TagId { get; private set; } = new(Guid.Empty);
    public PublicationId PublicationId { get; private set; } = new(Guid.Empty);
    public virtual Tag? Tag { get; }
    public virtual Publication? Publication { get; }
}