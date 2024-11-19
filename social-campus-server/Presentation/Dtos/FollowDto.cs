using Domain.Models.UserModel;

namespace Presentation.Dtos
{
    public record FollowDto(UserId UserId, UserId FollowUserId);
}
