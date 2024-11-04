namespace Application.Users.Commands.RefreshTokensCommand
{
    public record RefreshTokensCommandResponse(bool IsSuccess, string? AccessToken, string? RefreshToken, string? ErrorMessage);
}
