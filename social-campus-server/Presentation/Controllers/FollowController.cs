using Application.Follows.Commands.FollowCommand;
using Application.Follows.Commands.UnfollowCommand;
using Application.Follows.Queries.GetFollowersListQuery;
using Application.Follows.Queries.GetFollowingListQuery;
using Domain.Dtos;
using Domain.Models.UserModel;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController(
        IMediator mediator,
        IValidator<FollowCommandRequest> followValidator,
        IValidator<UnfollowCommandRequest> unfollowValidator,
        IValidator<GetFollowingListQueryRequest> getFollowingListValidator,
        IValidator<GetFollowersListQueryRequest> getFollowersListValidator) : ControllerBase
    {
        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromBody] FollowDto request)
        {
            FollowCommandRequest commandRequest = new(request.UserId, request.FollowUserId);

            ValidationResult result = await followValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
            }

            FollowCommandResponse response = await mediator.Send(commandRequest);
            if (!response.IsSuccess)
            {
                return Unauthorized(new { message = response.ErrorMessage });
            }

            return Ok(new { message = "Start following successfully." });
        }

        [HttpDelete("unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] FollowDto request)
        {
            UnfollowCommandRequest commandRequest = new(request.UserId, request.FollowUserId);

            ValidationResult result = await unfollowValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
            }

            UnfollowCommandResponse response = await mediator.Send(commandRequest);
            if (!response.IsSuccess)
            {
                return Unauthorized(new { message = response.ErrorMessage });
            }

            return Ok(new { message = "Stop following successfully." });
        }

        [HttpGet("{userId:guid}/following")]
        public async Task<ActionResult<IReadOnlyList<UserFollowDto?>>> GetFollowingList([FromRoute] Guid userId)
        {
            GetFollowingListQueryRequest commandRequest = new(new UserId(userId));

            ValidationResult result = await getFollowingListValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
            }

            IReadOnlyList<UserFollowDto?> response = await mediator.Send(commandRequest);
            if (!response.Any())
            {
                return Ok(new { message = "No following found." });
            }

            return Ok(response);
        }

        [HttpGet("{userId:guid}/followers")]
        public async Task<ActionResult<IReadOnlyList<UserFollowDto?>>> GetFollowersList([FromRoute] Guid userId)
        {
            GetFollowersListQueryRequest commandRequest = new(new UserId(userId));

            ValidationResult result = await getFollowersListValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
            }

            IReadOnlyList<UserFollowDto?> response = await mediator.Send(commandRequest);
            if (!response.Any())
            {
                return Ok(new { message = "No followers found." });
            }

            return Ok(response);
        }
    }
}
