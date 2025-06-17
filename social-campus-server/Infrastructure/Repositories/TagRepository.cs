using Domain.Abstractions.Repositories;
using Domain.Models.TagModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TagRepository(ApplicationDbContext context) : ITagRepository
{
    public async Task AddAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        await context.Tags.AddAsync(tag, cancellationToken);
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