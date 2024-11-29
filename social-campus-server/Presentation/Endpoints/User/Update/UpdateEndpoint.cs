using Application.Users.Commands.UpdateUser;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.User.Update
{
    public class UpdateEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("users/update", async (ISender sender, UpdateRequest request) =>
            {
                UpdateUserCommand commandRequest = new(
                    request.CallerId,
                    request.UserId,
                    request.Login,
                    request.Email,
                    request.FirstName,
                    request.LastName,
                    request.Bio,
                    request.ProfileImageData);

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.Users)
            .RequireAuthorization();
        }
    }
}
