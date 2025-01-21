using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.RefreshTokens.Commands.Refresh
{
    public record RefreshCommand(string RefreshToken) : ICommand<UserOnLoginRefreshDto>;
}
