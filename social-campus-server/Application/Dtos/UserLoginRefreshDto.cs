using Domain.Models.UserModel;

namespace Application.Dtos;

public class UserLoginRefreshDto
{
    public TokensDto Tokens { get; set; } = new();
    public UserId Id { get; set; } = new(Guid.Empty);
    public string Login { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ProfileImageUrl { get; set; } = string.Empty;
}