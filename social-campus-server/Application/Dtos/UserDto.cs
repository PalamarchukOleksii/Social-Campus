using Domain.Models.UserModel;

namespace Application.Dtos;

public class UserDto
{
    public UserId Id { get; set; } = new(Guid.Empty);
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; } = string.Empty;
    public IReadOnlyList<UserId>? FollowersIds { get; set; }
    public int FollowingCount { get; set; }
    public int FollowersCount { get; set; }
}