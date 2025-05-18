using Domain.Enums;
using Domain.Models.UserModel;

namespace Domain.Models.UserRestrictionModel
{
    public class UserRestriction
    {
        public UserRestrictionId Id { get; private set; } = new UserRestrictionId(Guid.Empty);
        public UserId TargetUserId { get; private set; } = new UserId(Guid.Empty);
        public virtual User? TargetUser { get; }
        public UserId ImposedByUserId { get; private set; } = new UserId(Guid.Empty);
        public virtual User? ImposedByUser { get; }
        public RestrictionType Type { get; }
        public string Reason { get; private set; } = string.Empty;
        public DateTime StartAt { get; private set; } = DateTime.UtcNow;
        public DateTime? EndAt { get; set; }
    }
}
