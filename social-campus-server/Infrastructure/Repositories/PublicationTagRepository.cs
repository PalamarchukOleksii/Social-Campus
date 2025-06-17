using Domain.Abstractions.Repositories;
using Domain.Models.PublicationModel;
using Domain.Models.PublicationTagModel;
using Domain.Models.TagModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PublicationTagRepository(ApplicationDbContext context) : IPublicationTagRepository
{
    public async Task<List<Publication>> GetPublicationsForTagAsync(TagId tagId, int page, int count,
        CancellationToken cancellationToken = default)
    {
        return await context.PublicationTags
            .Where(pt => pt.TagId == tagId)
            .Include(pt => pt.Publication)
            .ThenInclude(p => p.PublicationLikes)
            .Select(pt => pt.Publication!)
            .OrderByDescending(p => p.PublicationLikes!.Count)
            .Skip((page - 1) * count)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PublicationTag publicationTag, CancellationToken cancellationToken = default)
    {
        await context.PublicationTags.AddAsync(publicationTag, cancellationToken);
    }

    public void Remove(PublicationTag publicationTag)
    {
        context.PublicationTags.Remove(publicationTag);
    }

    public async Task<bool> ExistsAsync(PublicationId publicationId, TagId tagId,
        CancellationToken cancellationToken = default)
    {
        return await context.PublicationTags
            .AnyAsync(pt => pt.PublicationId == publicationId && pt.TagId == tagId, cancellationToken);
    }

    public async Task<List<Tag>> GetTagsForPublicationAsync(PublicationId publicationId,
        CancellationToken cancellationToken = default)
    {
        return await context.PublicationTags
            .Where(pt => pt.PublicationId == publicationId)
            .Include(pt => pt.Tag)
            .Select(pt => pt.Tag!)
            .ToListAsync(cancellationToken);
    }
}