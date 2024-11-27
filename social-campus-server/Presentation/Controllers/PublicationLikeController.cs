using Application.PublicationLikes.Commands.AddPublicationLike;
using Application.PublicationLikes.Commands.RemovePublicationLike;
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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PublicationLikeController(
        ISender sender,
        IValidator<AddPublicationLikeCommand> addLikeValidator,
        IValidator<RemovePublicationLikeCommand> removeLikeValidator) : ApiController(sender)
    {
        [HttpPost("publication/like/add")]
        public async Task<IActionResult> Follow([FromBody] PublicationLikeDto request)
        {
            AddPublicationLikeCommand commandRequest = new(request.UserId, request.PublicationId);

            ValidationResult result = await addLikeValidator.ValidateAsync(commandRequest);
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

        [HttpDelete("publication/like/remove")]
        public async Task<IActionResult> Unfollow([FromBody] PublicationLikeDto request)
        {
            RemovePublicationLikeCommand commandRequest = new(request.UserId, request.PublicationId);

            ValidationResult result = await removeLikeValidator.ValidateAsync(commandRequest);
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
