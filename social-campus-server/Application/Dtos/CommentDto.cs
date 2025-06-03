using Domain.Models.CommentModel;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.Dtos
{
    public class CommentDto
    {
        public CommentId Id { get; set; } = new CommentId(Guid.Empty);
        public string Description { get; set; } = string.Empty;
        public DateTime CreationDateTime { get; set; }
        public UserId CreatorId { get; set; } = new UserId(Guid.Empty);
        public PublicationId PublicationId { get; set; } = new PublicationId(Guid.Empty);
        public IReadOnlyList<UserId>? UserWhoLikedIds { get; set; }
    }
}
