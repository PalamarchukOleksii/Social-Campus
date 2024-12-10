using Application.CommentLikes.Commands.AddCommentLike;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.CommentLikes.AddCommentLike
{
    public class AddCommentLikeEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("commentlikes/add", async (ISender sender, AddCommentLikeRequest request) =>
            {
                AddCommentLikeCommand commandRequest = new(request.UserId, request.CommentId);

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.CommentLikes)
            .RequireAuthorization();
        }
    }
}
