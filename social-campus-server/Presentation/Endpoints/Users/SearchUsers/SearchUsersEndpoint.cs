using System;
using Application.Users.Queries.SearchUsers;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.SearchUsers;

public class SearchUsersEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/searchterm/{seatchterm}/count/{count:int}/page/{page:int}",
                async (ISender sender, string seatchterm, int count, int page) =>
                {
                    SearchUsersQuery queryRequest = new(seatchterm, page, count);

                    var response = await sender.Send(queryRequest);

                    return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
                })
            .WithTags(Tags.Users)
            .RequireAuthorization();
    }
}
