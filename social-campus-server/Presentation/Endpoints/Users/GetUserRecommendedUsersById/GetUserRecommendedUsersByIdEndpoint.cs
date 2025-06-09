using Application.Users.Queries.GetUserRecommendedUsers;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.GetUserRecommendedUsersById;

public class GetUserRecommendedUsersByIdEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{userId:guid:required}/recommended-users", async (ISender sender, Guid userId) =>
            {
                GetUserRecommendedUsersQuery queryRequest = new(new UserId(userId));

                var response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(Tags.Users)
            .RequireAuthorization();
    }
}