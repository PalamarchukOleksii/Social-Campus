namespace Application.RefreshTokens.Commands.RefreshCommand
{
    public record RefreshCommandResponse(bool IsSuccess, string? AccessToken, string? RefreshToken, string? ErrorMessage);
}
