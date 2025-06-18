using Application.Publications.Queries.GetPublicationsForTag;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.GetPublicationsForTagByLabel;

public class GetPublicationsForTagByLabelEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("publications/taglabel/{taglabel}/count/{count:int}/page/{page:int}", async (
                string taglabel,
                int count,
                int page,
                ISender sender) =>
            {
                var query = new GetPublicationsForTagQuery(taglabel, count, page);

                var response = await sender.Send(query);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(EndpointTags.Publications)
            .RequireAuthorization();
    }
}