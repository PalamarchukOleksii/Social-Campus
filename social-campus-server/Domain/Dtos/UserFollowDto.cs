using Domain.Models.UserModel;

namespace Domain.Dtos
{
    public class UserFollowDto
    {
        public UserId Id { get; set; } = new UserId(Guid.Empty);
        public string Login { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
    }
}
