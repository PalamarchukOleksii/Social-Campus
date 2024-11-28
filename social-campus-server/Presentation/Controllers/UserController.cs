using Application.Dtos;
using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetUserProfileByLogin;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Dtos;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(ISender sender) : ApiController(sender)
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            RegisterCommand commandRequest = new(
                request.Login,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password);

            Result response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok() : HandleFailure(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            LoginCommand commandRequest = new(request.Email, request.Password);

            Result<TokensDto> response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
        }

        [Authorize]
        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto request)
        {
            UpdateUserCommand commandRequest = new(
                request.CallerId,
                request.UserId,
                request.Login,
                request.Email,
                request.FirstName,
                request.LastName,
                request.Bio,
                request.ProfileImageData);

            Result response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok() : HandleFailure(response);
        }

        [Authorize]
        [HttpGet("{login}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfileByLogin([FromRoute] string login)
        {
            GetUserProfileByLoginQuery queryRequest = new(login);

            Result<UserProfileDto> response = await _sender.Send(queryRequest);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure<UserProfileDto>(response);
        }
    }
}
