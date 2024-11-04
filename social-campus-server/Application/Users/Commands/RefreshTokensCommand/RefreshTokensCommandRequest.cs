using MediatR;

namespace Application.Users.Commands.RefreshTokensCommand
{
    public record RefreshTokensCommandRequest(string AccessToken, string RefreshToken) : IRequest<RefreshTokensCommandResponse>;
}
