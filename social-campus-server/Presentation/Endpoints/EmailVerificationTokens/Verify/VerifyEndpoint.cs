using Application.EmailVerificationTokens.Commands.Verify;
using Domain.Consts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Presentation.Abstractions;
using Presentation.Consts;
using Presentation.Urls;

namespace Presentation.Endpoints.EmailVerificationTokens.Verify;

public class VerifyEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("verify-email-tokens/verify",
                async (ISender sender, Guid token, string email,
                    [FromServices] ApplicationUrlsOptions applicationUrls) =>
                {
                    var commandRequest = new VerifyCommand(token, email);
                    var response = await sender.Send(commandRequest);

                    if (response.IsSuccess)
                    {
                        var successUrl = QueryHelpers.AddQueryString(applicationUrls.Frontend.BaseUrl,
                            new Dictionary<string, string>
                            {
                                ["token"] = token.ToString(),
                                ["email"] = email
                            }!);

                        return Results.Redirect(successUrl);
                    }
                    else
                    {
                        var failureUrl = QueryHelpers.AddQueryString(applicationUrls.Frontend.BaseUrl,
                            new Dictionary<string, string>
                            {
                                ["errorMsg"] = response.Error.Message
                            }!);

                        return Results.Redirect(failureUrl);
                    }
                })
            .WithTags(EndpointTags.VerifyEmailTokens)
            .WithName(LinkConsts.VerifyEmail);
    }
}