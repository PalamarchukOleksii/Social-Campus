using Application.RefreshTokens.Commands.Revoke;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.RefreshTokens.Revoke;

public class RevokeEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("refreshtokens/revoke", async (HttpContext context, ISender sender) =>
            {
                if (!context.Request.Cookies.ContainsKey("RefreshToken") ||
                    !context.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
                    return Results.BadRequest("Refresh token is missing in request");
                if (string.IsNullOrEmpty(refreshToken))
                    return Results.BadRequest("Refresh token is null or empty");

                RevokeCommand commandRequest = new(refreshToken);

                var response = await sender.Send(commandRequest);
                if (response.IsSuccess)
                {
                    context.Response.Cookies.Delete("RefreshToken");

                    return Results.Ok();
                }

                return HandleFailure(response);
            })
            .WithTags(EndpointTags.RefreshTokens)
            .RequireAuthorization();
    }
}