using Domain.Models.UserModel;

namespace Domain.Models.UserRoleModel
{
    public class UserRole
    {
        public UserRoleId Id { get; private set; } = new UserRoleId(Guid.Empty);
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public virtual ICollection<User>? Users { get; }
    }
}