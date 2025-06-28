using Application.Dtos;
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
                FileUploadDto? fileDto = null;
                if (request.ProfileImage is not null)
                    fileDto = new FileUploadDto
                    {
                        Content = request.ProfileImage.OpenReadStream(),
                        FileName = request.ProfileImage.FileName,
                        ContentType = request.ProfileImage.ContentType
                    };

                var commandRequest = new UpdateUserCommand(
                    request.CallerId,
                    request.UserId,
                    request.Login,
                    request.FirstName,
                    request.LastName,
                    request.Bio,
                    fileDto);

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .Accepts<UpdateUserRequest>("multipart/form-data")
            .WithTags(EndpointTags.Users)
            .RequireAuthorization()
            .DisableAntiforgery();
    }
}