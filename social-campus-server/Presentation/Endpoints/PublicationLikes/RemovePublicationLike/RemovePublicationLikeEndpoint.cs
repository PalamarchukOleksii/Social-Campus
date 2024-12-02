using Application.PublicationLikes.Commands.RemovePublicationLike;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.PublicationLikes.RemovePublicationLike
{
    public class RemovePublicationLikeEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("publicationlikes/remove/{publicationId:guid:required}/{userId:guid:required}",
                async (ISender sender, Guid publicationId, Guid userId) =>
            {
                RemovePublicationLikeCommand commandRequest = new(new UserId(userId), new PublicationId(publicationId));

                Result response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .WithTags(Tags.PublicationLikes)
            .RequireAuthorization();
        }
    }
}
