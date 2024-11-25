using Application.Dtos;
using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.Commands.Update;
using Domain.Shared;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Dtos;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(
        IValidator<RegisterCommand> registerValidator,
        IValidator<LoginCommand> loginValidator,
        IValidator<UpdateUserCommand> updateValidator,
        ISender sender) : ApiController(sender)
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            RegisterCommand commandRequest = new(
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

            Result response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok() : BadRequest(response.Error);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            LoginCommand commandRequest = new(request.Email, request.Password);

            ValidationResult result = await loginValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
            }

            Result<TokensDto> response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto request)
        {
            UpdateUserCommand commandRequest = new(
                request.Id,
                request.Login,
                request.Email,
                request.FirstName,
                request.LastName,
                request.Bio,
                request.ProfileImageData);

            ValidationResult result = await updateValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
            }

            Result response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok() : BadRequest(response.Error);
        }
    }
}
