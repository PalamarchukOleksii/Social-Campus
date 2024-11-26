using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories
{
    public interface IPublicationRepository
    {
        public Task AddAsync(string description, UserId creatorId, string imageData);
        public Task<Publication?> GetByIdAsync(PublicationId publicationId);
        public Task<IReadOnlyList<Publication>> GetUserPublicationsByUserIdAsync(UserId creatorId);
    }
}
