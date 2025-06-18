using Application.Publications.Queries.GetHomePagePublications;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.GetHomePagePublicationsByUserId;

public class GetHomePagePublicationsByUserIdEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("publications/home/user/{userId:guid}/count/{count:int}/page/{page:int}", async (
                Guid userId,
                int count,
                int page,
                ISender sender) =>
            {
                var query = new GetHomePagePublicationsQuery(
                    new UserId(userId),
                    page,
                    count);

                var response = await sender.Send(query);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(EndpointTags.Publications)
            .RequireAuthorization();
    }
}