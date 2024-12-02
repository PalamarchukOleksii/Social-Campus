using Application.Publications.Commands.UpdatePublication;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.UpdatePublication
{
    public class UpdatePublicationEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("publications/update", async (ISender sender, UpdatePublicationRequest request) =>
            {
                UpdatePublicationCommand commandRequest = new(
                    request.CallerId,
                    request.PublicationId,
                    request.Description,
                    request.ImageData);

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.Publications)
            .RequireAuthorization();
        }
    }
}
