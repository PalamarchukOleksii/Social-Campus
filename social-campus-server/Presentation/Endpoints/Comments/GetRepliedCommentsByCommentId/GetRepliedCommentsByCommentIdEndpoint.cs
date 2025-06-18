using Application.Comments.Queries.GetRepliesToComment;
using Domain.Models.CommentModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Comments.GetRepliedCommentsByCommentId;

public class GetRepliedCommentsByCommentIdEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("comments/replies/{commentId:guid:required}", async (ISender sender, Guid commentId) =>
            {
                GetRepliesToCommentQuery queryRequest = new(new CommentId(commentId));

                var response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(EndpointTags.Comments)
            .RequireAuthorization();
    }
}