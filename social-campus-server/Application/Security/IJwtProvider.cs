using Domain.Models;

namespace Application.Security
{
    public interface IJwtProvider
    {
        string CreateToken(User user);
    }
}
