using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.RefreshTokens.Commands.Refresh
{
    public record RefreshCommand(string AccessToken, string RefreshToken) : ICommand<UserOnLoginRefreshDto>;
}
