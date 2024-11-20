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
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
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
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
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
