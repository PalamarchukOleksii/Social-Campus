using Domain.Models.UserModel;

namespace Presentation.Endpoints.Publications.CreatePublication;

public class CreatePublicationRequest
{
    public string Description { get; set; } = string.Empty;
    public UserId CreatorId { get; set; } = new(Guid.Empty);
    public IFormFile? ImageData { get; set; }
}