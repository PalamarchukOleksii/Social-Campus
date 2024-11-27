using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.Dtos
{
    public class LikeDto
    {
        public PublicationId PublicationId { get; set; } = new PublicationId(Guid.Empty);
        public UserId UserId { get; set; } = new UserId(Guid.Empty);
    }
}
