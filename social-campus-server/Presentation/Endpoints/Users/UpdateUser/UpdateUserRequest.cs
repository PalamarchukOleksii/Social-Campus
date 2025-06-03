using Domain.Models.UserModel;

namespace Presentation.Endpoints.Users.UpdateUser;

public class UpdateUserRequest
{
    public UserId CallerId { get; set; } = new(Guid.Empty);
    public UserId UserId { get; set; } = new(Guid.Empty);
    public string Login { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string ProfileImageData { get; set; } = string.Empty;
}