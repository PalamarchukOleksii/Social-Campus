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
                if (!context.Request.Cookies.ContainsKey("RefreshToken") || !context.Request.Cookies.TryGetValue("RefreshToken", out string? refreshToken))
                {
                    return Results.BadRequest("Refresh token is missing in request");
                }
                else if (string.IsNullOrEmpty(refreshToken))
                {
                    return Results.BadRequest("Refresh token is null or empty");
                }

                RefreshCommand commandRequest = new(request.AccessToken, refreshToken);

                Result<UserOnLoginRefreshDto> response = await sender.Send(commandRequest);
                if (response.IsSuccess)
                {
                    TokensDto tokens = response.Value.Tokens;

                    context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTimeOffset.UtcNow.AddSeconds(tokens.RefreshTokenExpirationInSeconds),
                    });

                    ShortUserDto shortUser = response.Value.ShortUser;

                    return Results.Ok(new
                    {
                        shortUser,
                        tokens.AccessToken,
                        tokens.AccessTokenExpirationInSeconds
                    });
                }

                return HandleFailure(response);
            }).WithTags(Tags.RefreshTokens);
        }
    }
}
