using Application.ResetPasswordTokens.Commands.Generate;
using Domain.Consts;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.ResetPasswordTokens.Generate;

public class GenerateEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("reset-password-tokens/generate", async (ISender sender, GenerateRequest request) =>
            {
                var commandRequest = new GenerateCommand(request.Email);

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(EndpointTags.ResetPasswordTokens)
            .WithName(LinkConsts.ResetPassword);
    }
}