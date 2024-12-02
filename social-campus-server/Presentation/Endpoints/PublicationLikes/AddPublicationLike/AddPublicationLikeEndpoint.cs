using Application.PublicationLikes.Commands.AddPublicationLike;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.PublicationLikes.AddPublicationLike
{
    public class AddPublicationLikeEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("publicationlikes/add", async (ISender sender, AddPublicationLikeRequest request) =>
            {
                AddPublicationLikeCommand commandRequest = new(request.UserId, request.PublicationId);

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.PublicationLikes)
            .RequireAuthorization();
        }
    }
}
