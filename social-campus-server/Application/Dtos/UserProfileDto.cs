using Domain.Models.UserModel;

namespace Application.Dtos
{
    public class UserProfileDto
    {
        public UserId Id { get; set; } = new UserId(Guid.Empty);
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ProfileImageData { get; set; } = string.Empty;
        public IReadOnlyList<ShortPublicationDto>? Publications { get; set; }
        public int FollowingCount { get; set; }
        public int FollowersCount { get; set; }
    }
}
