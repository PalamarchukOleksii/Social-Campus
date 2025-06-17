using Domain.Models.PublicationTagModel;
using Domain.Models.PublicationModel;
using Domain.Models.TagModel;

namespace Domain.Abstractions.Repositories;

public interface IPublicationTagRepository
{
    Task<List<Publication>> GetPublicationsForTagAsync(TagId tagId, int page, int count,
        CancellationToken cancellationToken = default);

    Task AddAsync(TagId tagId, PublicationId publicationId, CancellationToken cancellationToken = default);
    void Remove(PublicationTag publicationTag);
    Task<bool> ExistsAsync(PublicationId publicationId, TagId tagId, CancellationToken cancellationToken = default);

    public Task<List<Tag>> GetTagsForPublicationAsync(PublicationId publicationId,
        CancellationToken cancellationToken = default);

    public Task RemoveAsync(TagId tagId, PublicationId publicationId, CancellationToken cancellationToken = default);
}