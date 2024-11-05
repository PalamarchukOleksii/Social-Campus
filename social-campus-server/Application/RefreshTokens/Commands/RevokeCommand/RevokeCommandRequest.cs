using MediatR;

namespace Application.RefreshTokens.Commands.RevokeCommand
{
    public record RevokeCommandRequest(string RefreshToken) : IRequest<RevokeCommandResponse>;
}
