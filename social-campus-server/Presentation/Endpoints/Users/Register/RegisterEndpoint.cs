using Application.Users.Commands.Register;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Users.Register
{
    public class RegisterEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users/register", async (ISender sender, RegisterRequest request) =>
            {
                RegisterCommand commandRequest = new(
                    request.Login,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Password);

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            }).WithTags(Tags.Users);
        }
    }
}
