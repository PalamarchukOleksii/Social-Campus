using Application.CommentLikes.Commands.RemoveCommentLike;
using Domain.Models.CommentModel;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.CommentLikes.RemoveCommentLike;

public class RemoveCommentLikeEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("commentlikes/remove/{commentId:guid:required}/{userId:guid:required}",
                async (ISender sender, Guid commentId, Guid userId) =>
                {
                    RemoveCommentLikeCommand commandRequest = new(new UserId(userId), new CommentId(commentId));

                    var response = await sender.Send(commandRequest);

                    return response.IsSuccess ? Results.Ok() : HandleFailure(response);
                })
            .WithTags(EndpointTags.CommentLikes)
            .RequireAuthorization();
    }
}