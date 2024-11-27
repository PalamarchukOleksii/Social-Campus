using Application.Dtos;
using Application.Publications.Commands.CreatePublication;
using Application.Publications.Queries.GetPublication;
using Domain.Models.PublicationModel;
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
    public class PublicationController(
        ISender sender,
        IValidator<CreatePublicationCommand> createPublicationValidator,
        IValidator<GetPublicationQuery> getPublicationValidator) : ApiController(sender)
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreatePublicationDto request)
        {
            CreatePublicationCommand commandRequest = new(request.Description, request.CreatorId, request.ImageData);

            ValidationResult result = await createPublicationValidator.ValidateAsync(commandRequest);
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

        [HttpGet("publication/{publicationId:guid}")]
        public async Task<ActionResult<ShortPublicationDto>> GetPublication([FromRoute] Guid publicationId)
        {
            GetPublicationQuery queryRequest = new(new PublicationId(publicationId));

            ValidationResult result = await getPublicationValidator.ValidateAsync(queryRequest);
            if (!result.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
            }

            Result<ShortPublicationDto> response = await _sender.Send(queryRequest);

            return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
        }
    }
}
