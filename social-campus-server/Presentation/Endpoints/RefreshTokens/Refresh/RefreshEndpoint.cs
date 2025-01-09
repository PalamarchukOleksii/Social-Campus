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
            app.MapPost("refreshtokens/refresh", async (HttpContext context, ISender sender, RefreshRequest request) =>
            {
                if (!context.Request.Cookies.TryGetValue("RefreshToken", out string? refreshToken) || string.IsNullOrEmpty(refreshToken))
                {
                    return Results.BadRequest("Refresh token is missing or invalid");
                }

                RefreshCommand commandRequest = new(request.AccessToken, refreshToken);

                Result<TokensDto> response = await sender.Send(commandRequest);
                if (response.IsSuccess)
                {
                    TokensDto tokens = response.Value;

                    context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(tokens.RefreshTokenExpirationInDays)
                    });

                    return Results.Ok(new
                    {
                        tokens.AccessToken,
                        tokens.AccessTokenExpirationInMinutes
                    });
                }

                return HandleFailure(response);
            }).WithTags(Tags.RefreshTokens);
        }
    }
}
