using Application.Dtos;
using Application.Users.Commands.Login;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.Login
{
    public class LoginEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users/login", async (HttpContext context, ISender sender, LoginRequest request) =>
            {
                LoginCommand commandRequest = new(request.Email, request.Password);

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
            }).WithTags(Tags.Users);
        }
    }
}
