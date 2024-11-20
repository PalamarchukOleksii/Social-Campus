using Domain.Models.UserModel;

namespace Presentation.Dtos
{
    public class CreatePublicationDto
    {
        public string Description { get; set; } = string.Empty;
        public UserId CreatorId { get; set; } = new UserId(Guid.Empty);
        public string? Base64ImageData { get; set; }
    }
}
