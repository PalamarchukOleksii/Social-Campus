using Application.RefreshTokens.Commands.RefreshCommand;
using Application.RefreshTokens.Commands.RevokeCommand;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefreshTokenController(
        IValidator<RefreshCommandRequest> refreshValidator,
        IValidator<RevokeCommandRequest> revokeValidator,
        IMediator mediator) : ControllerBase
    {
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto request)
        {
            RefreshCommandRequest commandRequest = new(request.AccessToken, request.RefreshToken);

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

            RefreshCommandResponse response = await mediator.Send(commandRequest);
            if (!response.IsSuccess)
            {
                return Unauthorized(new { message = response.ErrorMessage });
            }

            return Ok(new { response.AccessToken, response.RefreshToken });
        }

        [Authorize]
        [HttpDelete("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RevokeDto request)
        {
            RevokeCommandRequest commandRequest = new(request.RefreshToken);

            ValidationResult result = await revokeValidator.ValidateAsync(commandRequest);
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

            RevokeCommandResponse response = await mediator.Send(commandRequest);
            if (!response.IsSuccess)
            {
                return Unauthorized(new { message = response.ErrorMessage });
            }

            return Ok();
        }
    }
}
