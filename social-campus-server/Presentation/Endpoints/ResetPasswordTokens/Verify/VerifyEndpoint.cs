using Application.ResetPasswordTokens.Commands.Verify;
using Domain.Consts;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.ResetPasswordTokens.Verify;

public class VerifyEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("reset-password-tokens/verify", async (ISender sender, Guid token, Guid userId) =>
            {
                var commandRequest = new VerifyCommand(token, new UserId(userId));
                var response = await sender.Send(commandRequest);

                string baseUrl = "http://localhost:3000/forgot-password";

                if (response.IsSuccess)
                {
                    var successUrl = $"{baseUrl}?token={token}&userId={userId}";
                    return Results.Redirect(successUrl);
                }
                else
                {
                    var error = Uri.EscapeDataString(response.Error.Message);
                    var failureUrl = $"{baseUrl}?errorMsg={error}";
                    return Results.Redirect(failureUrl);
                }
            })
            .WithTags(EndpointTags.ResetPasswordTokens)
            .WithName(LinkConsts.ResetPassword);
    }
}
