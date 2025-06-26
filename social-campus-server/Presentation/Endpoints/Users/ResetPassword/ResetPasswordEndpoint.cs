using Application.Users.Commands.ResetPassword;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.ResetPassword;

public class ResetPasswordEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/reset-password", async (ISender sender, ResetPasswordRequest request) =>
            {
                var commandRequest = new ResetPasswordCommand(request.ResetPasswordToken,
                    new UserId(request.ResetPasswordToken), request.NewPassword);

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(EndpointTags.Users);
    }
}