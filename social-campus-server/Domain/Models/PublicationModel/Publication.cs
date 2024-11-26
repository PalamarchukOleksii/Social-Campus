using Domain.Models.PublicationLikeModel;
using Domain.Models.UserModel;

namespace Domain.Models.PublicationModel
{
    public class Publication
    {
        private Publication() { }

        public Publication(string description, UserId creatorId, string imageData)
        {
            Id = new PublicationId(Guid.NewGuid());
            Description = description;
            ImageData = imageData;
            CreatorId = creatorId;
            CreationDateTime = DateTime.UtcNow;
        }

        public PublicationId Id { get; private set; } = new PublicationId(Guid.Empty);
        public string Description { get; private set; } = string.Empty;
        public string ImageData { get; private set; } = string.Empty;
        public DateTime CreationDateTime { get; private set; }
        public UserId CreatorId { get; private set; } = new UserId(Guid.Empty);
        public virtual User? Creator { get; }
        public virtual ICollection<PublicationLike>? PublicationLikes { get; }
    }
}
