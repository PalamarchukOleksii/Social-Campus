using Application.Dtos;
using Application.Follows.Commands.Follow;
using Application.Follows.Commands.Unfollow;
using Application.Follows.Queries.GetFollowersList;
using Application.Follows.Queries.GetFollowingList;
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
    public class FollowController(ISender sender) : ApiController(sender)
    {
        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromBody] FollowDto request)
        {
            FollowCommand commandRequest = new(request.UserLogin, request.FollowUserLogin);

            Result response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok() : HandleFailure(response);
        }

        [HttpDelete("unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] FollowDto request)
        {
            UnfollowCommand commandRequest = new(request.UserLogin, request.FollowUserLogin);

            Result response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok() : HandleFailure(response);
        }

        [HttpGet("{login}/following")]
        public async Task<ActionResult<IReadOnlyList<ShortUserDto>>> GetFollowingList([FromRoute] string login)
        {
            GetFollowingListQuery queryRequest = new(login);

            Result<IReadOnlyList<ShortUserDto>> response = await _sender.Send(queryRequest);
            if (response.IsFailure)
            {
                return HandleFailure<IReadOnlyList<ShortUserDto>>(response);
            }

            if (response.Value == null || !response.Value.Any())
            {
                return Ok(new { message = "No following found" });
            }

            return Ok(response.Value);
        }

        [HttpGet("{login}/followers")]
        public async Task<ActionResult<IReadOnlyList<ShortUserDto>>> GetFollowersList([FromRoute] string login)
        {
            GetFollowersListQuery queryRequest = new(login);

            Result<IReadOnlyList<ShortUserDto>> response = await _sender.Send(queryRequest);
            if (response.IsFailure)
            {
                return HandleFailure<IReadOnlyList<ShortUserDto>>(response);
            }

            if (response.Value == null || !response.Value.Any())
            {
                return Ok(new { message = "No followers found" });
            }

            return Ok(response.Value);
        }
    }
}
