using Domain.Models.UserModel;

namespace Domain.Models.FollowModel;

public class Follow
{
    private Follow()
    {
    }

    public Follow(UserId userId, UserId followUserId)
    {
        Id = new FollowId(Guid.NewGuid());
        UserId = userId;
        FollowedUserId = followUserId;
    }

    public FollowId Id { get; private set; } = new(Guid.Empty);
    public UserId UserId { get; private set; } = new(Guid.Empty);
    public virtual User? User { get; }
    public UserId FollowedUserId { get; private set; } = new(Guid.Empty);
    public virtual User? FollowedUser { get; }
}