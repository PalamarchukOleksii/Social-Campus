using Domain.Models.PublicationLikeModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Domain.Abstractions.Repositories
{
    public interface IPublicationLikeRepositories
    {
        public Task AddAsync(UserId userId, PublicationId publicationId);
        public Task DeleteAsync(UserId userId, PublicationId publicationId);
        public Task<IReadOnlyList<PublicationLike>> GetPublicationLikesListByPublicationIdAsync(PublicationId publicationId);
        public Task<bool> IsLike(UserId userId, PublicationId publicationId);
    }
}
