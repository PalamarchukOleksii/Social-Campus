using Domain.Models.UserModel;

namespace Presentation.Dtos
{
    public class FollowDto
    {
        public UserId UserId { get; set; } = new UserId(Guid.Empty);
        public UserId FollowUserId { get; set; } = new UserId(Guid.Empty);
    }
}
