using Application.Users.Commands.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.UpdateUser;

public class UpdateUserEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("users/update", async (ISender sender, [FromForm] UpdateUserRequest request) =>
            {
                var commandRequest = new UpdateUserCommand(
                    request.CallerId,
                    request.UserId,
                    request.Login,
                    request.Email,
                    request.FirstName,
                    request.LastName,
                    request.Bio,
                    request.ProfileImage);

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .Accepts<UpdateUserRequest>("multipart/form-data")
            .WithTags(Tags.Users)
            .RequireAuthorization()
            .DisableAntiforgery();
    }
}