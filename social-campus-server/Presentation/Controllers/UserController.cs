using Application.Users.Commands.LoginUserCommand;
using Application.Users.Commands.RegisterUserCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommandRequest request)
        {
            var response = await mediator.Send(request);

            if (!response.IsSuccess)
                return BadRequest(new { Message = response.ErrorMessage });

            return Ok(new { Message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommandRequest request)
        {
            var response = await mediator.Send(request);

            if (!response.IsSuccess)
                return Unauthorized(new { Message = response.ErrorMessage });

            return Ok(new { AccessToken = response.AccessToken });
        }
    }
}
