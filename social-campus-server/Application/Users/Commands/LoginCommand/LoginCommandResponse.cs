namespace Application.Users.Commands.LoginCommand
{
    public record LoginCommandResponse(bool IsSuccess, string? AccessToken, string? RefreshToken, string? ErrorMessage);
}
