using Application.Publications.Commands.DeletePublication;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.DeletePublication;

public class DeletePublicationEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("publications/delete/{publicationId:guid}/{callerId:guid}",
                async (ISender sender, Guid publicationId, Guid callerId) =>
                {
                    var commandRequest =
                        new DeletePublicationCommand(new PublicationId(publicationId), new UserId(callerId));

                    var response = await sender.Send(commandRequest);

                    return response.IsSuccess ? Results.Ok() : HandleFailure(response);
                })
            .WithTags(EndpointTags.Publications)
            .RequireAuthorization();
    }
}