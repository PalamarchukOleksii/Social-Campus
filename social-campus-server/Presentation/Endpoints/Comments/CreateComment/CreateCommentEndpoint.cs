﻿using Application.Comments.Commands.CreateComment;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Comments.CreateComment
{
    public class CreateCommentEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("comments/create", async (ISender sender, CreateCommentRequest request) =>
            {
                CreateCommentCommand commandRequest = new(request.PublicationId, request.Description, request.CreatorId);

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.Comments)
            .RequireAuthorization();
        }
    }
}
