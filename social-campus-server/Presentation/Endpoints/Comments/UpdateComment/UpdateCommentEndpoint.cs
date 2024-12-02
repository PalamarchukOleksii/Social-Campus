using Application.Comments.Commands.UpdateComment;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Comments.UpdateComment
{
    public class UpdateCommentEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("comments/update", async (ISender sender, UpdateCommentRequest request) =>
            {
                UpdateCommentCommand commandRequest = new(request.CallerId, request.CommentId, request.Description);

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.Comments)
            .RequireAuthorization();
        }
    }
}
