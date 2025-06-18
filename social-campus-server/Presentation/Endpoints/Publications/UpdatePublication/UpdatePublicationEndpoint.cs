using Application.Publications.Commands.UpdatePublication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.UpdatePublication;

public class UpdatePublicationEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("publications/update", async (ISender sender, [FromForm] UpdatePublicationRequest request) =>
            {
                UpdatePublicationCommand commandRequest = new(
                    request.CallerId,
                    request.PublicationId,
                    request.Description,
                    request.ImageData);

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .Accepts<UpdatePublicationRequest>("multipart/form-data")
            .WithTags(EndpointTags.Publications)
            .RequireAuthorization()
            .DisableAntiforgery();
    }
}