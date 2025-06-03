using Domain.Models.UserModel;

namespace Application.Dtos
{
    public class UserLoginRefreshDto
    {
        public TokensDto Tokens { get; set; } = new TokensDto();
        public UserId Id { get; set; } = new UserId(Guid.Empty);
        public string Login { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ProfileImageData { get; set; } = string.Empty;
    }
}
