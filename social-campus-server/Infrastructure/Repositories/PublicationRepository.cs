using Domain.Abstractions.Repositories;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PublicationRepository(ApplicationDbContext context) : IPublicationRepository
{
    public async Task<Publication> AddAsync(string description, UserId creatorId, string imageData)
    {
        Publication newPublication = new(description, creatorId, imageData);

        await context.Publications.AddAsync(newPublication);

        return newPublication;
    }

    public async Task<IReadOnlyList<Publication>> GetUserPublicationsByUserIdAsync(
        UserId creatorId,
        Publication? lastPublication,
        int count)
    {
        var query = context.Publications
            .Where(p => p.CreatorId == creatorId);

        if (lastPublication != null) query = query.Where(p => p.CreationDateTime < lastPublication.CreationDateTime);

        return await query
            .OrderByDescending(p => p.CreationDateTime)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Publication?> GetByIdAsync(PublicationId publicationId)
    {
        return await context.Publications.FirstOrDefaultAsync(p => p.Id == publicationId);
    }

    public void Update(Publication publication, string description, string imageData)
    {
        publication.Update(description, imageData);

        context.Update(publication);
    }

    public async Task<bool> IsExistByIdAsync(PublicationId publicationId)
    {
        return await context.Publications.AnyAsync(u => u.Id == publicationId);
    }

    public async Task<IReadOnlyList<Publication>> GetPublicationsForHomePageAsync(
        IReadOnlyList<User> followedUsers,
        int page,
        int count,
        User currentUser)
    {
        var userIds = followedUsers.Select(u => u.Id).ToHashSet();
        userIds.Add(currentUser.Id);

        var totalFollowedCount = await context.Publications
            .Where(p => userIds.Contains(p.CreatorId))
            .CountAsync();

        var totalFollowedPages = (int)Math.Ceiling((double)totalFollowedCount / count);

        if (page <= totalFollowedPages && totalFollowedPages > 0)
        {
            var followedPublications = await context.Publications
                .Where(p => userIds.Contains(p.CreatorId))
                .OrderByDescending(p => p.CreationDateTime)
                .Skip((page - 1) * count)
                .Take(count)
                .ToListAsync();

            return followedPublications;
        }

        var randomPage = page - totalFollowedPages;
        if (randomPage < 1) randomPage = 1;

        var randomPublications = await context.Publications
            .Where(p => !userIds.Contains(p.CreatorId))
            .OrderByDescending(p => p.CreationDateTime)
            .Skip((randomPage - 1) * count)
            .Take(count)
            .ToListAsync();

        return randomPublications;
    }
}