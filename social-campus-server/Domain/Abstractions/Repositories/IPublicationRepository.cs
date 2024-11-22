using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories
{
    public interface IPublicationRepository
    {
        public Task AddAsync(string description, UserId creatorId, string imageData);
        public Task<Publication?> GetPublicationByIdAsync(PublicationId publicationId);
        public Task<IReadOnlyList<Publication>> GetAllPublicationsByUserIdasync(UserId creatorId);
    }
}
