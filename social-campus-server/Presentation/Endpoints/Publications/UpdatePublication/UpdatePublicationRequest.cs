using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Presentation.Endpoints.Publications.UpdatePublication
{
    public class UpdatePublicationRequest
    {
        public UserId CallerId { get; set; } = new UserId(Guid.Empty);
        public PublicationId PublicationId { get; set; } = new PublicationId(Guid.Empty);
        public string Description { get; set; } = string.Empty;
        public string ImageData { get; set; } = string.Empty;
    }
}
