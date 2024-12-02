using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Presentation.Endpoints.PublicationLikes.AddLike
{
    public class AddPublicationLikeRequest
    {
        public UserId UserId { get; set; } = new UserId(Guid.Empty);
        public PublicationId PublicationId { get; set; } = new PublicationId(Guid.Empty);
    }
}
