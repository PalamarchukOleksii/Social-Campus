using Application.Publications.Commands.CreatePublication;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.CreatePublication
{
    public class CreatePublicationEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("publications/create", async (ISender sender, CreatePublicationRequest request) =>
            {
                CreatePublicationCommand commandRequest = new(request.Description, request.CreatorId, request.ImageData);

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.Publications)
            .RequireAuthorization();
        }
    }
}
