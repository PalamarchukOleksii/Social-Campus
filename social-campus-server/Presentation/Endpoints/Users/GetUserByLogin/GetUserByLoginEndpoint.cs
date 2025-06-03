using Application.Users.Queries.GetUserByLogin;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.GetUserByLogin;

public class GetUserByLoginEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/by-login/{login:required}", async (ISender sender, string login) =>
            {
                GetUserByLoginQuery queryRequest = new(login);

                var response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(Tags.Users)
            .RequireAuthorization();
    }
}