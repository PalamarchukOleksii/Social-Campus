using Domain.Abstractions.Repositories;
using Domain.Models.TagModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TagRepository(ApplicationDbContext context) : ITagRepository
{
    public async Task<Tag?> GetByIdAsync(TagId tagId)
    {
        return await context.Tags.FirstOrDefaultAsync(x => x.Id.Value == tagId.Value);
    }

    public async Task<Tag?> GetByLabelAsync(string label)
    {
        return await context.Tags.FirstOrDefaultAsync(t => t.Label == label);
    }

    public async Task<Tag> AddAsync(string label, CancellationToken cancellationToken = default)
    {
        var newTag = new Tag(label);

        await context.Tags.AddAsync(newTag, cancellationToken);

        return newTag;
    }

    public async Task<bool> ExistsAsync(string label, CancellationToken cancellationToken = default)
    {
        return await context.Tags.AnyAsync(t => t.Label == label, cancellationToken);
    }

    public async Task<IReadOnlyList<Tag>> SearchAsync(string searchTerm, int page, int count)
    {
        searchTerm = $"%{searchTerm.ToLower().Trim()}%";

        var users = await context.Tags
            .Where(u =>
                EF.Functions.Like(u.Label.ToLower(), searchTerm))
            .Include(u => u.PublicationTags)
            .Skip((page - 1) * count)
            .Take(count)
            .ToListAsync();

        return users
            .OrderByDescending(u => u.PublicationTags?.Count ?? 0)
            .ToList();
    }
}