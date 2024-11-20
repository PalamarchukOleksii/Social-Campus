using Application.Users.Commands.LoginCommand;
using Application.Users.Commands.RegisterCommand;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(
        IMediator mediator,
        IValidator<RegisterCommandRequest> registerValidator,
        IValidator<LoginCommandRequest> loginValidator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            RegisterCommandRequest commandRequest = new(
                request.Login,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password);

            ValidationResult result = await registerValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
            }

            await mediator.Send(commandRequest);

            return Ok(new { message = "Registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            LoginCommandRequest commandRequest = new(request.Email, request.Password);

            ValidationResult result = await loginValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
            }

            LoginCommandResponse response = await mediator.Send(commandRequest);
            if (!response.IsSuccess)
            {
                return Unauthorized(new { message = response.ErrorMessage });
            }

            return Ok(new { response.AccessToken, response.RefreshToken });
        }
    }
}
