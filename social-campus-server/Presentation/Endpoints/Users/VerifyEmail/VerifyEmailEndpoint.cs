using Application.Users.Commands.VerifyEmail;
using Domain.Consts;
using Domain.Models.EmailVerificationTokenModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.VerifyEmail;

public class VerifyEmailEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/verify-email", async (ISender sender, Guid token) =>
            {
                var commandRequest = new VerifyEmailCommand(new EmailVerificationTokenId(token));

                var response = await sender.Send(commandRequest);

                if (response.IsSuccess) return Results.Redirect("/VerifyEmailSuccess");

                var errorMsg = Uri.EscapeDataString(response.Error.Message);
                return Results.Redirect($"/VerifyEmailFailed?error={errorMsg}");
            })
            .WithTags(EndpointTags.Users)
            .WithName(VerifyConsts.VerifyEmail);
    }
}