using Application.Dtos;
using Application.Publications.Queries.GetPublication;
using Domain.Models.PublicationModel;
using Domain.Shared;
using MediatR;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.GetPublicationById
{
    public class GetPublicationByIdEndpoint : BaseEndpoint, IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("publications/{publicationId:guid:required}", async (ISender sender, Guid publicationId) =>
            {
                GetPublicationQuery queryRequest = new(new PublicationId(publicationId));

                Result<ShortPublicationDto> response = await sender.Send(queryRequest);

                return response.IsSuccess ? Results.Ok(response.Value) : HandleFailure(response);
            })
            .WithTags(Tags.Publications)
            .RequireAuthorization();
        }
    }
}
