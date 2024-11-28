using Application.Dtos;
using Application.Publications.Commands.CreatePublication;
using Application.Publications.Commands.UpdatePublication;
using Application.Publications.Queries.GetPublication;
using Domain.Models.PublicationModel;
using Domain.Shared;
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
    public class PublicationController(ISender sender) : ApiController(sender)
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreatePublicationDto request)
        {
            CreatePublicationCommand commandRequest = new(request.Description, request.CreatorId, request.ImageData);

            Result response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok() : HandleFailure(response);
        }

        [HttpGet("publication/{publicationId:guid}")]
        public async Task<ActionResult<ShortPublicationDto>> GetPublication([FromRoute] Guid publicationId)
        {
            GetPublicationQuery queryRequest = new(new PublicationId(publicationId));

            Result<ShortPublicationDto> response = await _sender.Send(queryRequest);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure<ShortPublicationDto>(response);
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] UpdatePublicationDto updatePublicationDto)
        {
            UpdatePublicationCommand commandRequest = new(
                updatePublicationDto.CallerId,
                updatePublicationDto.PublicationId,
                updatePublicationDto.Description,
                updatePublicationDto.ImageData);

            Result response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok() : HandleFailure(response);
        }
    }
}
