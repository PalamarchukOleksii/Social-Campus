using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Presentation.Endpoints.Comments.CreateComment
{
    public class CreateCommentRequest
    {
        public PublicationId PublicationId { get; set; } = new PublicationId(Guid.Empty);
        public string Description { get; set; } = string.Empty;
        public UserId CreatorId { get; set; } = new UserId(Guid.Empty);
    }
}
