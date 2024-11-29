using Application.Dtos;
using Application.Users.Queries.GetUserProfileByLogin;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.User.GetProfileByLogin
{
    public class GetProfileByLoginEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("users/{login}", async (ISender sender, string login) =>
            {
                GetUserProfileByLoginQuery queryRequest = new(login);

                Result<UserProfileDto> response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(Tags.Users)
            .RequireAuthorization();
        }
    }
}
