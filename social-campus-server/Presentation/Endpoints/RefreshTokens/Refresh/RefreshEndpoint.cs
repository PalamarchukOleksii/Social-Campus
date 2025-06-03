using Application.Dtos;
using Application.RefreshTokens.Commands.Refresh;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.RefreshTokens.Refresh;

public class RefreshEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("refreshtokens/refresh", async (HttpContext context, ISender sender) =>
        {
            if (!context.Request.Cookies.ContainsKey("RefreshToken") ||
                !context.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
                return Results.BadRequest("Refresh token is missing in request");

            if (string.IsNullOrEmpty(refreshToken)) return Results.BadRequest("Refresh token is null or empty");

            RefreshCommand commandRequest = new(refreshToken);

            var response = await sender.Send(commandRequest);
            if (!response.IsSuccess) return HandleFailure(response);

            var tokens = response.Value.Tokens;

            context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddSeconds(tokens.RefreshTokenExpirationInSeconds)
            });

            var shortUser = new UserDto
            {
                Id = response.Value.Id,
                Login = response.Value.Login,
                FirstName = response.Value.FirstName,
                LastName = response.Value.LastName,
                ProfileImageUrl = response.Value.ProfileImageUrl
            };

            return Results.Ok(new
            {
                shortUser,
                tokens.AccessToken,
                tokens.AccessTokenExpirationInSeconds
            });
        }).WithTags(Tags.RefreshTokens);
    }
}