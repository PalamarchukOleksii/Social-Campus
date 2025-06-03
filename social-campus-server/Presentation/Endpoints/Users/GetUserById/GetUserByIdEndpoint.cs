using Application.Users.Queries.GetUserById;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.GetUserById;

public class GetUserByIdEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/by-id/{userId:guid:required}", async (ISender sender, Guid userId) =>
            {
                GetUserByIdQuery queryRequest = new(new UserId(userId));

                var response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(Tags.Users)
            .RequireAuthorization();
    }
}