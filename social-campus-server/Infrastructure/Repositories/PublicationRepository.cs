using Domain.Abstractions.Repositories;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PublicationRepository(ApplicationDbContext context) : IPublicationRepository
{
    public async Task AddAsync(string description, UserId creatorId, string imageData)
    {
        Publication newPublication = new(description, creatorId, imageData);

        await context.Publications.AddAsync(newPublication);
    }

    public async Task<IReadOnlyList<Publication>> GetUserPublicationsByUserIdAsync(UserId creatorId)
    {
        return await context.Publications
            .Where(p => p.CreatorId == creatorId)
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
        int limit,
        User currentUser)
    {
        var userIds = followedUsers.Select(u => u.Id).ToList();

        if (!userIds.Contains(currentUser.Id))
            userIds.Add(currentUser.Id);

        return await context.Publications
            .Where(p => userIds.Contains(p.CreatorId) || EF.Functions.Random() < 0.1)
            .OrderByDescending(p => p.CreationDateTime)
            .Where(p => lastPublication == null || p.CreationDateTime < lastPublication.CreationDateTime)
            .Take(limit)
            .ToListAsync();
    }
}