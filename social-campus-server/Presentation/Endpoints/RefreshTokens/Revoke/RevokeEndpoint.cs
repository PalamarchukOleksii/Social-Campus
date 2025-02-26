﻿using Application.RefreshTokens.Commands.Revoke;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.RefreshTokens.Revoke
{
    public class RevokeEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("refreshtokens/revoke", async (HttpContext context, ISender sender) =>
            {
                if (!context.Request.Cookies.ContainsKey("RefreshToken") || !context.Request.Cookies.TryGetValue("RefreshToken", out string? refreshToken))
                {
                    return Results.BadRequest("Refresh token is missing in request");
                }
                else if (string.IsNullOrEmpty(refreshToken))
                {
                    return Results.BadRequest("Refresh token is null or empty");
                }

                RevokeCommand commandRequest = new(refreshToken);

                Result response = await sender.Send(commandRequest);
                if (response.IsSuccess)
                {
                    context.Response.Cookies.Delete("RefreshToken");

                    return Results.Ok();
                }

                return HandleFailure(response);
            })
            .WithTags(Tags.RefreshTokens)
            .RequireAuthorization();
        }
    }
}
