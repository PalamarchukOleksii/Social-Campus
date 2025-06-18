using Application.Users.Queries.GetUserPublications;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.GetUserPublications;

public class GetUserPublicationsEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{userId:guid:required}/publications/count/{count:int}",
                async (ISender sender, Guid userId, int count, Guid? lastPublicationId) =>
                {
                    GetUserPublicationsQuery queryRequest =
                        new(new UserId(userId),
                            lastPublicationId is not null ? new PublicationId(lastPublicationId.Value) : null, count);

                    var response = await sender.Send(queryRequest);

                    return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
                })
            .WithTags(EndpointTags.Users)
            .RequireAuthorization();
    }
}