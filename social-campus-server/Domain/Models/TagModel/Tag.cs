using Domain.Models.PublicationTagModel;

namespace Domain.Models.TagModel;

public class Tag
{
    private Tag()
    {
    }

    public Tag(string label)
    {
        Id = new TagId(Guid.NewGuid());
        Label = label;
    }

    public TagId Id { get; private set; } = new(Guid.Empty);
    public string Label { get; private set; } = string.Empty;
    public virtual ICollection<PublicationTag>? PublicationTags { get; }
}