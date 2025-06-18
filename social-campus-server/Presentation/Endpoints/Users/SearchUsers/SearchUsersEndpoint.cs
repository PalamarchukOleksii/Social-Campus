using Application.Users.Queries.SearchUsers;
using MediatR;
using Presentation.Abstractions;

namespace Presentation.Endpoints.Users.SearchUsers;

public class SearchUsersEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/searchterm/{searchterm}/count/{count:int}/page/{page:int}",
                async (ISender sender, string searchterm, int count, int page) =>
                {
                    SearchUsersQuery queryRequest = new(searchterm, page, count);

                    var response = await sender.Send(queryRequest);

                    return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
                })
            .WithTags(Consts.EndpointTags.Users)
            .RequireAuthorization();
    }
}