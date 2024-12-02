using Application.Comments.Queries.GetRepliesToComment;
using Application.Dtos;
using Domain.Models.CommentModel;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Comments.GetRepliedCommentsByCommentId
{
    public class GetRepliedCommentsByCommentIdEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("comments/replies/{commentId:guid:required}", async (ISender sender, Guid commentId) =>
            {
                GetRepliesToCommentQuery queryRequest = new(new CommentId(commentId));

                Result<IReadOnlyList<CommentDto>> response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(Tags.Comments)
            .RequireAuthorization();
        }
    }
}
