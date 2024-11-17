using MediatR;

namespace Application.RefreshTokens.Commands.RefreshCommand
{
    public record RefreshCommandRequest(string AccessToken, string RefreshToken) : IRequest<RefreshCommandResponse>;
}
