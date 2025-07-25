using Application.ResetPasswordTokens.Commands.Verify;
using Domain.Consts;
using Domain.Models.UserModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Presentation.Abstractions;
using Presentation.Consts;
using Presentation.Options;

namespace Presentation.Endpoints.ResetPasswordTokens.Verify;

public class VerifyEndpoint : BaseEndpoint, IEndpoint
{
    private const string ForgotPasswordClientRelevantPath = "/forgot-password";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("reset-password-tokens/verify",
                async (ISender sender, Guid token, Guid userId,
                    [FromServices] ApplicationUrlsOptions applicationUrls) =>
                {
                    var commandRequest = new VerifyCommand(token, new UserId(userId));
                    var response = await sender.Send(commandRequest);

                    var uri = new Uri(new Uri(applicationUrls.ClientBaseUrl.TrimEnd('/')),
                        ForgotPasswordClientRelevantPath.TrimStart('/'));
                    if (response.IsSuccess)
                    {
                        var successUrl = QueryHelpers.AddQueryString(uri.ToString(),
                            new Dictionary<string, string>
                            {
                                ["token"] = token.ToString(),
                                ["userId"] = userId.ToString()
                            }!);

                        return Results.Redirect(successUrl);
                    }

                    var failureUrl = QueryHelpers.AddQueryString(uri.ToString(),
                        new Dictionary<string, string>
                        {
                            ["errorMsg"] = response.Error.Message
                        }!);

                    return Results.Redirect(failureUrl);
                })
            .WithTags(EndpointTags.ResetPasswordTokens)
            .WithName(LinkConsts.ResetPassword);
    }
}