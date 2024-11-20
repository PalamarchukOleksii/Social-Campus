using Domain.Abstractions.Repositories;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PublicationRepository(ApplicationDbContext context) : IPublicationRepository
    {
        public async Task AddAsync(string description, UserId creatorId, string? base64ImageData)
        {
            Publication newPublication = new(description, creatorId, base64ImageData);

            await context.Publications.AddAsync(newPublication);
        }

        public async Task<IReadOnlyList<Publication>> GetAllPublicationsByUserIdasync(UserId creatorId)
        {
            return await context.Publications
                .Where(p => p.CreatorId == creatorId)
                .ToListAsync() as IReadOnlyList<Publication>;
        }

        public async Task<Publication?> GetPublicationByIdAsync(PublicationId publicationId)
        {
            return await context.Publications.FirstOrDefaultAsync(p => p.Id == publicationId);
        }
    }
}
