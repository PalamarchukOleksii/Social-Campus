using Application.Comments.Queries.GetComment;
using Domain.Models.CommentModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Comments.GetCommentById;

public class GetCommentByIdEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("comments/{commentId:guid:required}", async (ISender sender, Guid commentId) =>
            {
                GetCommentQuery queryRequest = new(new CommentId(commentId));

                var response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(EndpointTags.Comments)
            .RequireAuthorization();
    }
}