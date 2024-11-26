using Domain.Abstractions.Repositories;
using Domain.Models.PublicationLikeModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PublicationLikeRepositories(
        ApplicationDbContext context) : IPublicationLikeRepositories
    {
        public async Task AddAsync(UserId userId, PublicationId publicationId)
        {
            PublicationLike newPublicationLike = new(userId, publicationId);

            await context.PublicationLikes.AddAsync(newPublicationLike);
        }

        public async Task DeleteAsync(UserId userId, PublicationId publicationId)
        {
            PublicationLike? publicationLike = await context.PublicationLikes
                .FirstOrDefaultAsync(pl => pl.PublicationId == publicationId && pl.UserId == userId);
            if (publicationLike != null)
            {
                context.PublicationLikes.Remove(publicationLike);
            }
        }

        public async Task<IReadOnlyList<Publication>> GetPublicationLikesUsersByIdAsync(PublicationId publicationId)
        {
            return await context.PublicationLikes
                .Where(pl => pl.PublicationId == publicationId)
                .Select(p => p.Publication)
                .Where(publication => publication != null)
                .ToListAsync() as IReadOnlyList<Publication>;
        }

        public async Task<bool> IsLike(UserId userId, PublicationId publicationId)
        {
            return await context.PublicationLikes.AnyAsync(pl => pl.PublicationId == publicationId && pl.UserId == userId);
        }
    }
}
