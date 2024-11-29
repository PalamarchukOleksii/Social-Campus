using Application.Dtos;
using Application.RefreshTokens.Commands.Refresh;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.RefreshTokens.Refresh
{
    public class RefreshEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("refreshtokens/refresh", async (ISender sender, RefreshRequest request) =>
            {
                RefreshCommand commandRequest = new(request.AccessToken, request.RefreshToken);

                Result<TokensDto> response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(Tags.RefreshTokens);
        }
    }
}
