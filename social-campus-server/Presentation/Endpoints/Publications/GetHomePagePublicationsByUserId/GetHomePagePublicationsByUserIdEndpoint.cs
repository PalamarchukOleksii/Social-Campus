using Application.Publications.Queries.GetHomePagePublications;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.GetHomePagePublicationsByUserId;

public class GetHomePagePublicationsByUserIdEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("publications/home/user/{userId:guid}/{count:int}", async (
                Guid userId,
                int count,
                Guid? lastPublicationId,
                ISender sender) =>
            {
                var query = new GetHomePagePublicationsQuery(
                    new UserId(userId),
                    lastPublicationId is not null ? new PublicationId(lastPublicationId.Value) : null,
                    count);

                var response = await sender.Send(query);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(Tags.Publications)
            .RequireAuthorization();
    }
}