using Application.Dtos;
using Application.Follows.Commands.Follow;
using Application.Follows.Commands.Unfollow;
using Application.Follows.Queries.GetFollowersList;
using Application.Follows.Queries.GetFollowingList;
using Domain.Models.UserModel;
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
    public class FollowController(
        ISender sender,
        IValidator<FollowCommand> followValidator,
        IValidator<UnfollowCommand> unfollowValidator,
        IValidator<GetFollowingListQuery> getFollowingListValidator,
        IValidator<GetFollowersListQuery> getFollowersListValidator) : ApiController(sender)
    {
        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromBody] FollowDto request)
        {
            FollowCommand commandRequest = new(request.UserId, request.FollowUserId);

            ValidationResult result = await followValidator.ValidateAsync(commandRequest);
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

        [HttpDelete("unfollow")]
        public async Task<IActionResult> Unfollow([FromBody] FollowDto request)
        {
            UnfollowCommand commandRequest = new(request.UserId, request.FollowUserId);

            ValidationResult result = await unfollowValidator.ValidateAsync(commandRequest);
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

        [HttpGet("{userId:guid}/following")]
        public async Task<ActionResult<IReadOnlyList<UserFollowDto?>>> GetFollowingList([FromRoute] Guid userId)
        {
            GetFollowingListQuery commandRequest = new(new UserId(userId));

            ValidationResult result = await getFollowingListValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
            }

            Result<IReadOnlyList<UserFollowDto?>> response = await _sender.Send(commandRequest);

            if (response.IsFailure)
            {
                return BadRequest(response.Error);
            }

            if (response.Value == null || !response.Value.Any())
            {
                return Ok(new { message = "No following found" });
            }

            return Ok(response.Value);
        }

        [HttpGet("{userId:guid}/followers")]
        public async Task<ActionResult<IReadOnlyList<UserFollowDto?>>> GetFollowersList([FromRoute] Guid userId)
        {
            GetFollowersListQuery commandRequest = new(new UserId(userId));

            ValidationResult result = await getFollowersListValidator.ValidateAsync(commandRequest);
            if (!result.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ));
            }

            Result<IReadOnlyList<UserFollowDto?>> response = await _sender.Send(commandRequest);
            if (response.IsFailure)
            {
                return BadRequest(response.Error);
            }

            if (response.Value == null || !response.Value.Any())
            {
                return Ok(new { message = "No followers found" });
            }

            return Ok(response.Value);
        }
    }
}
