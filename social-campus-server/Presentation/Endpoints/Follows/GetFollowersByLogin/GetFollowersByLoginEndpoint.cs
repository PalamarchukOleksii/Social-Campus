using Application.Dtos;
using Application.Follows.Queries.GetFollowersList;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Follows.GetFollowersByLogin
{
    public class GetFollowersByLoginEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("follows/{login:required}/followers", async (ISender sender, string login) =>
            {
                GetFollowersListQuery queryRequest = new(login);

                Result<IReadOnlyList<UserDto>> response = await sender.Send(queryRequest);
                if (response.IsFailure)
                {
                    return HandleFailure(response);
                }

                return Results.Ok(response.Value);
            })
            .WithTags(Tags.Follows)
            .RequireAuthorization();
        }
    }
}
