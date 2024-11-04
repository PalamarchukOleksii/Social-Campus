using Application.Users.Commands.LoginUserCommand;
using Application.Users.Commands.RefreshTokensCommand;
using Application.Users.Commands.RegisterUserCommand;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;

namespace Presentation.Controllers
{
    [ApiController]
    public class UserController(
        IMediator mediator,
        IValidator<RegisterUserCommandRequest> registerValidator,
        IValidator<LoginUserCommandRequest> loginValidator,
        IValidator<RefreshTokensCommandRequest> refreshValidator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            RegisterUserCommandRequest commandRequest = new RegisterUserCommandRequest(
                request.Login,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password);

            ValidationResult result = await registerValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(failure => failure.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(failure => failure.ErrorMessage).ToArray()
                    );

                return ValidationProblem(new ValidationProblemDetails(errors));
            }

            await mediator.Send(commandRequest);

            return Ok(new { message = "Registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            LoginUserCommandRequest commandRequest = new LoginUserCommandRequest(request.Email, request.Password);

            ValidationResult result = await loginValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(failure => failure.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(failure => failure.ErrorMessage).ToArray()
                    );

                return ValidationProblem(new ValidationProblemDetails(errors));
            }

            LoginUserCommandResponse response = await mediator.Send(commandRequest);
            if (!response.IsSuccess)
            {
                return Unauthorized(new { message = response.ErrorMessage });
            }

            return Ok(new { response.AccessToken, response.RefreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokensDto request)
        {
            RefreshTokensCommandRequest commandRequest = new RefreshTokensCommandRequest(request.AccessToken, request.RefreshToken);

            ValidationResult result = await refreshValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(failure => failure.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(failure => failure.ErrorMessage).ToArray()
                    );

                return ValidationProblem(new ValidationProblemDetails(errors));
            }

            RefreshTokensCommandResponse response = await mediator.Send(commandRequest);
            if (!response.IsSuccess)
            {
                return Unauthorized(new { message = response.ErrorMessage });
            }

            return Ok(new { response.AccessToken, response.RefreshToken });
        }
    }
}
