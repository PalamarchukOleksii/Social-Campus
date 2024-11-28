using Application.Dtos;
using Application.RefreshTokens.Commands.Refresh;
using Application.RefreshTokens.Commands.Revoke;
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
    public class RefreshTokenController(ISender sender) : ApiController(sender)
    {
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto request)
        {
            RefreshCommand commandRequest = new(request.AccessToken, request.RefreshToken);

            Result<TokensDto> response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok(response.Value) : HandleFailure(response);
        }

        [Authorize]
        [HttpDelete("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RevokeDto request)
        {
            RevokeCommand commandRequest = new(request.RefreshToken);

            Result response = await _sender.Send(commandRequest);

            return response.IsSuccess ? Ok() : HandleFailure(response);
        }
    }
}
