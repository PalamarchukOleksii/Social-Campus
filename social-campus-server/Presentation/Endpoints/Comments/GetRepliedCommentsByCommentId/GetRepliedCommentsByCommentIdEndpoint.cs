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
        app.MapGet("comments/replies/{commentId:guid:required}/count/{count:int}/page/{page:int}", async (
                ISender sender, Guid commentId, int count, int page) =>
            {
                GetRepliesToCommentQuery queryRequest = new(new CommentId(commentId), page, count);

                var response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(EndpointTags.Comments)
            .RequireAuthorization();
    }
}