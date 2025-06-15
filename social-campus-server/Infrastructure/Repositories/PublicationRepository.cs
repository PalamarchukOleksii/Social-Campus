using Domain.Abstractions.Repositories;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PublicationRepository(ApplicationDbContext context) : IPublicationRepository
{
    private static readonly Random Random = new();

    public async Task AddAsync(string description, UserId creatorId, string imageData)
    {
        Publication newPublication = new(description, creatorId, imageData);

        await context.Publications.AddAsync(newPublication);
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
        Publication? lastPublication,
        int count,
        User currentUser)
    {
        var userIds = followedUsers.Select(u => u.Id).ToHashSet();
        userIds.Add(currentUser.Id);

        var randomPart = Random.NextDouble() * 0.5;
        var randomCount = (int)(count * randomPart);
        randomCount = Math.Min(randomCount, count);
        var followedCountLimit = count - randomCount;

        var followedQuery = context.Publications
            .Where(p => userIds.Contains(p.CreatorId));

        if (lastPublication != null)
            followedQuery = followedQuery.Where(p => p.CreationDateTime < lastPublication.CreationDateTime);

        var followingPublications = await followedQuery
            .OrderByDescending(p => p.CreationDateTime)
            .Take(followedCountLimit)
            .ToListAsync();

        var needCount = count - followingPublications.Count;
        if (needCount > 0)
        {
            var randomPublicationsQuery = context.Publications
                .Where(p => !userIds.Contains(p.CreatorId));

            if (lastPublication != null)
                randomPublicationsQuery =
                    randomPublicationsQuery.Where(p => p.CreationDateTime < lastPublication.CreationDateTime);

            var randomPublications = await randomPublicationsQuery
                .Take(needCount * 5)
                .ToListAsync();

            randomPublications = randomPublications
                .OrderBy(_ => Random.Next())
                .Take(needCount)
                .ToList();

            followingPublications.AddRange(randomPublications);
        }

        return followingPublications
            .OrderByDescending(p => p.CreationDateTime)
            .Take(count)
            .ToList();
    }
}