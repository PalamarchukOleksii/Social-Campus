namespace Presentation.Dtos
{
    public record RegisterDto
    (
        string Login,
        string FirstName,
        string LastName,
        string Email,
        string Password
    );
}
