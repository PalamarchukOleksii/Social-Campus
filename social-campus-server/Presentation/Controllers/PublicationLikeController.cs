using Application.PublicationLikes.Commands.AddPublicationLike;
using Application.PublicationLikes.Commands.RemovePublicationLike;
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
    public class PublicationLikeController(ISender sender) : ApiController(sender)
    {
        [HttpPost("like/add")]
        public async Task<IActionResult> Follow([FromBody] PublicationLikeDto request)
        {
            AddPublicationLikeCommand commandRequest = new(request.UserId, request.PublicationId);

            Result response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok() : HandleFailure(response);
        }

        [HttpDelete("like/remove")]
        public async Task<IActionResult> Unfollow([FromBody] PublicationLikeDto request)
        {
            RemovePublicationLikeCommand commandRequest = new(request.UserId, request.PublicationId);

            Result response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok() : HandleFailure(response);
        }
    }
}
