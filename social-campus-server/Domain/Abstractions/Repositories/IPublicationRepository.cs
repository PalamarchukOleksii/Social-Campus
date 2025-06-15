using Domain.Models.FollowModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories;

public interface IPublicationRepository
{
    public Task AddAsync(string description, UserId creatorId, string imageData);
    public Task<Publication?> GetByIdAsync(PublicationId publicationId);

    public Task<IReadOnlyList<Publication>> GetUserPublicationsByUserIdAsync(UserId creatorId,
        Publication? lastPublication, int limit);

    public void Update(Publication publication, string description, string imageData);
    public Task<bool> IsExistByIdAsync(PublicationId publicationId);

    public Task<IReadOnlyList<Publication>> GetPublicationsForHomePageAsync(IReadOnlyList<User> followedUsers,
        Publication? lastPublication, int limit, User currentUser);
}