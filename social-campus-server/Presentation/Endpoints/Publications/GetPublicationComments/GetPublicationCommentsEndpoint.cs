using Application.Publications.Queries.GetPublicationComments;
using Domain.Models.PublicationModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.GetPublicationComments;

public class GetPublicationCommentsEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("publications/{publicationId:guid:required}/comments", async (ISender sender, Guid publicationId) =>
            {
                GetPublicationCommentsQuery queryRequest = new(new PublicationId(publicationId));

                var response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(EndpointTags.Publications)
            .RequireAuthorization();
    }
}