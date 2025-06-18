using Application.Follows.Queries.GetFollowersList;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Follows.GetFollowersByLogin;

public class GetFollowersByLoginEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("follows/{login:required}/followers", async (ISender sender, string login) =>
            {
                GetFollowersListQuery queryRequest = new(login);

                var response = await sender.Send(queryRequest);
                if (response.IsFailure) return HandleFailure(response);

                return Results.Ok(response.Value);
            })
            .WithTags(EndpointTags.Follows)
            .RequireAuthorization();
    }
}