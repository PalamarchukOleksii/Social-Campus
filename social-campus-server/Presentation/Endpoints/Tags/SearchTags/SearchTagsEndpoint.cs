using Application.Tags.Queries.SearchTags;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Tags.SearchTags;

public class SearchTagsEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/tags/searchterm/{searchterm}/count/{count:int}/page/{page:int}",
                async (ISender sender, string searchterm, int count, int page) =>
                {
                    SearchTagsQuery queryRequest = new(searchterm, page, count);

                    var response = await sender.Send(queryRequest);

                    return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
                })
            .WithTags(EndpointTags.Tags)
            .RequireAuthorization();
    }
}