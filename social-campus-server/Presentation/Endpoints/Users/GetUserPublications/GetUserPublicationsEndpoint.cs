using Application.Users.Queries.GetUserPublications;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.GetUserPublications;

public class GetUserPublicationsEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{userId:guid:required}/publications", async (ISender sender, Guid userId) =>
            {
                GetUserPublicationsQuery queryRequest = new(new UserId(userId));

                var response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(Tags.Users)
            .RequireAuthorization();
    }
}