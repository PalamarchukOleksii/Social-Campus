using Application.Abstractions.Messaging;
using Domain.Models.TokensModel;

namespace Application.RefreshTokens.Commands.Refresh
{
    public record RefreshCommand(string AccessToken, string RefreshToken) : ICommand<Tokens>;
}
