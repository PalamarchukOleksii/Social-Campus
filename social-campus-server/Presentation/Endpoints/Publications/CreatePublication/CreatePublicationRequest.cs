using Domain.Models.UserModel;

namespace Presentation.Endpoints.Publications.CreatePublication
{
    public class CreatePublicationRequest
    {
        public string Description { get; set; } = string.Empty;
        public UserId CreatorId { get; set; } = new UserId(Guid.Empty);
        public string ImageData { get; set; } = string.Empty;
    }
}
