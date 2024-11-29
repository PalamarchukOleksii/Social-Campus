using Application.RefreshTokens.Commands.Revoke;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.RefreshToken.Revoke
{
    public class RevokeEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("resreshtokens/revoke", async (ISender sender, string refreshToken) =>
            {
                RevokeCommand commandRequest = new(refreshToken);

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.ResreshTokens)
            .RequireAuthorization();
        }
    }
}
