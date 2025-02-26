﻿using Application.Follows.Commands.Follow;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Follows.Follow
{
    public class FollowEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("follows/follow", async (ISender sender, FollowRequest request) =>
            {
                FollowCommand commandRequest = new(request.UserLogin, request.FollowUserLogin);

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.Follows)
            .RequireAuthorization();
        }
    }
}
