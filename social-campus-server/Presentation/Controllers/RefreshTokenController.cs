using Application.Dtos;
using Application.RefreshTokens.Commands.Refresh;
using Application.RefreshTokens.Commands.Revoke;
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
    public class RefreshTokenController(
        IValidator<RefreshCommand> refreshValidator,
        IValidator<RevokeCommand> revokeValidator,
        ISender sender) : ApiController(sender)
    {
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto request)
        {
            RefreshCommand commandRequest = new(request.AccessToken, request.RefreshToken);

            ValidationResult result = await refreshValidator.ValidateAsync(commandRequest);
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
        [HttpDelete("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RevokeDto request)
        {
            RevokeCommand commandRequest = new(request.RefreshToken);

            ValidationResult result = await revokeValidator.ValidateAsync(commandRequest);
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
