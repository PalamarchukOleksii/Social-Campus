using Application.Dtos;
using Application.Users.Commands.Login;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.User.Login
{
    public class LoginEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users/login", async (ISender sender, LoginRequest request) =>
            {
                LoginCommand commandRequest = new(request.Email, request.Password);

                Result<TokensDto> response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            }).WithTags(Tags.Users);
        }
    }
}
