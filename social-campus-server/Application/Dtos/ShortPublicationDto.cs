using Domain.Models.PublicationModel;

namespace Application.Dtos
{
    public class ShortPublicationDto
    {
        public PublicationId Id { get; set; } = new PublicationId(Guid.Empty);
        public string Description { get; set; } = string.Empty;
        public string ImageData { get; set; } = string.Empty;
        public DateTime CreationDateTime { get; set; }
    }
}
