using Application.Follows.Queries.GetFollowingList;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Follows.GetFollowingByLogin;

public class GetFollowingByLoginEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("follows/{login:required}/following", async (ISender sender, string login) =>
            {
                GetFollowingListQuery queryRequest = new(login);

                var response = await sender.Send(queryRequest);
                if (response.IsFailure) return HandleFailure(response);

                return Results.Ok(response.Value);
            })
            .WithTags(EndpointTags.Follows)
            .RequireAuthorization();
    }
}