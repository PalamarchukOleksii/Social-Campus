using Application.Comments.Commands.ReplyToComment;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Comments.ReplyToComment;

public class ReplyToCommentEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("comments/reply", async (ISender sender, ReplyToCommentRequest request) =>
            {
                ReplyToCommentCommand commandRequest = new(
                    request.PublicationId,
                    request.ReplyToCommentId,
                    request.Description,
                    request.CreatorId);

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(EndpointTags.Comments)
            .RequireAuthorization();
    }
}