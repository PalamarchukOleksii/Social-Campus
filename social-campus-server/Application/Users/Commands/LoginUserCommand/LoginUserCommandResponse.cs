namespace Application.Users.Commands.LoginUserCommand
{
    public record LoginUserCommandResponse(bool IsSuccess, string? AccessToken, string? RefreshToken, string? ErrorMessage);
}
