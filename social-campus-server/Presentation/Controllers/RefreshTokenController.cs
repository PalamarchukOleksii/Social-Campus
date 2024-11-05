using Application.Users.Commands.RefreshTokensCommand;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefreshTokenController(
        IValidator<RefreshTokensCommandRequest> refreshValidator,
        IMediator mediator) : ControllerBase
    {
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokensDto request)
        {
            RefreshTokensCommandRequest commandRequest = new(request.AccessToken, request.RefreshToken);

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
