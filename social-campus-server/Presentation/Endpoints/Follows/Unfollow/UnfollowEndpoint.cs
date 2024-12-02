using Application.Follows.Commands.Unfollow;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Follows.Unfollow
{
    public class UnfollowEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("follows/unfollow/{userLogin:required}/{followUserLogin:required}",
                async (ISender sender, string userLogin, string followUserLogin) =>
            {
                UnfollowCommand commandRequest = new(userLogin, followUserLogin);

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.Follows)
            .RequireAuthorization();
        }
    }
}
