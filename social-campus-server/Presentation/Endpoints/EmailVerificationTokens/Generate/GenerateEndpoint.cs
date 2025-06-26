using Application.EmailVerificationTokens.Commands.Generate;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.EmailVerificationTokens.Generate;

public class GenerateEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("verify-email-tokens/generate", async (ISender sender, GenerateRequest request) =>
            {
                var commandRequest = new GenerateCommand(request.Email);

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(EndpointTags.VerifyEmailTokens);
    }
}