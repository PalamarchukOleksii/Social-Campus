using Application.Publications.Commands.CreatePublication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.CreatePublication;

public class CreatePublicationEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("publications/create", async (ISender sender, [FromForm] CreatePublicationRequest request) =>
            {
                CreatePublicationCommand commandRequest =
                    new(request.Description, request.CreatorId, request.ImageData);

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .Accepts<CreatePublicationRequest>("multipart/form-data")
            .WithTags(Tags.Publications)
            .RequireAuthorization()
            .DisableAntiforgery();
    }
}