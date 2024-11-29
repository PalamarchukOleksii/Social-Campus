using Domain.Models.UserModel;

namespace Presentation.Endpoints.User.Update
{
    public class UpdateRequest
    {
        public UserId CallerId { get; set; } = new UserId(Guid.Empty);
        public UserId UserId { get; set; } = new UserId(Guid.Empty);
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ProfileImageData { get; set; } = string.Empty;
    }
}
