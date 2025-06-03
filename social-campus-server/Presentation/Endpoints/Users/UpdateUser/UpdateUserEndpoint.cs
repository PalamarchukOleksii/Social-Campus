using Application.Users.Commands.UpdateUser;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.UpdateUser;

public class UpdateUserEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("users/update", async (ISender sender, UpdateUserRequest request) =>
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

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.Users)
            .RequireAuthorization();
    }
}