using Application.Users.Commands.DeleteUser;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.DeleteUser;

public class DeleteUserEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("users/delete/{userId:guid}/{callerId:guid}", async (ISender sender, Guid userId, Guid callerId) =>
            {
                var commandRequest = new DeleteUserCommand(new UserId(userId), new UserId(callerId));

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(EndpointTags.Users)
            .RequireAuthorization();
    }
}