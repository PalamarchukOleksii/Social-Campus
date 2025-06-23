using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories;

public interface IPublicationRepository
{
    public Task<Publication> AddAsync(string description, UserId creatorId, string imageData);
    public Task<Publication?> GetByIdAsync(PublicationId publicationId);

    public Task<IReadOnlyList<Publication>> GetUserPublicationsByUserIdAsync(UserId creatorId,
        Publication? lastPublication, int count);

    public void Update(Publication publication, string description, string imageData);
    public Task<bool> IsExistByIdAsync(PublicationId publicationId);

    public Task<IReadOnlyList<Publication>> GetPublicationsForHomePageAsync(IReadOnlyList<User> followedUsers,
        int page, int count, User currentUser);

    public void Delete(Publication publication);
}