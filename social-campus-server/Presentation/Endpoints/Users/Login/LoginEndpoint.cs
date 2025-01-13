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

                Result<UserOnLoginDto> response = await sender.Send(commandRequest);
                if (response.IsSuccess)
                {
                    TokensDto tokens = response.Value.Tokens;

                    context.Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTimeOffset.UtcNow.AddDays(tokens.RefreshTokenExpirationInDays),
                    });

                    ShortUserDto shortUser = response.Value.ShortUser;

                    return Results.Ok(new
                    {
                        shortUser,
                        tokens.AccessToken,
                        tokens.AccessTokenExpirationInMinutes
                    });
                }

                return HandleFailure(response);
            }).WithTags(Tags.Users);
        }
    }
}
