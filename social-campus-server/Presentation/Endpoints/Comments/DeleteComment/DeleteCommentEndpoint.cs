using Application.Comments.Commands.DeleteComment;
using Domain.Models.CommentModel;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Comments.DeleteComment;

public class DeleteCommentEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("comments/delete/{commentId:guid}/{callerId:guid}",
                async (ISender sender, Guid commentId, Guid callerId) =>
                {
                    var commandRequest =
                        new DeleteCommentCommand(new CommentId(commentId), new UserId(callerId));

                    var response = await sender.Send(commandRequest);

                    return response.IsSuccess ? Results.Ok() : HandleFailure(response);
                })
            .WithTags(EndpointTags.Comments)
            .RequireAuthorization();
    }
}