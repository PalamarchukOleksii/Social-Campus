using Application.Abstractions.Messaging;
using Domain.Dtos;

namespace Application.RefreshTokens.Commands.Refresh
{
    public record RefreshCommand(string AccessToken, string RefreshToken) : ICommand<TokensDto>;
}
