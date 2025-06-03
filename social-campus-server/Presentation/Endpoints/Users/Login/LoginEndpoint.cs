using Application.Dtos;
using Application.Users.Commands.Login;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.Login;

public class LoginEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", async (HttpContext context, ISender sender, LoginRequest request) =>
        {
            LoginCommand commandRequest = new(request.Email, request.Password);

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
                ProfileImageData = response.Value.ProfileImageData
            };

            return Results.Ok(new
            {
                shortUser,
                tokens.AccessToken,
                tokens.AccessTokenExpirationInSeconds
            });
        }).WithTags(Tags.Users);
    }
}