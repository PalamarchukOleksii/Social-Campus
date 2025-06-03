using Application.Users.Queries.GetUserByLogin;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.GetProfileByLogin;

public class GetProfileByLoginEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{login:required}", async (ISender sender, string login) =>
            {
                GetUserByLoginQuery queryRequest = new(login);

                var response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(Tags.Users)
            .RequireAuthorization();
    }
}