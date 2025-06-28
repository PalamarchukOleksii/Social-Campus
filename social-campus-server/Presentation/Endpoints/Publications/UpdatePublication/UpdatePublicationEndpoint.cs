using Application.Dtos;
using Application.Publications.Commands.UpdatePublication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.UpdatePublication;

public class UpdatePublicationEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("publications/update", async (ISender sender, [FromForm] UpdatePublicationRequest request) =>
            {
                FileUploadDto? fileDto = null;
                if (request.ImageData is not null)
                    fileDto = new FileUploadDto
                    {
                        Content = request.ImageData.OpenReadStream(),
                        FileName = request.ImageData.FileName,
                        ContentType = request.ImageData.ContentType
                    };

                UpdatePublicationCommand commandRequest = new(
                    request.CallerId,
                    request.PublicationId,
                    request.Description,
                    fileDto);

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .Accepts<UpdatePublicationRequest>("multipart/form-data")
            .WithTags(EndpointTags.Publications)
            .RequireAuthorization()
            .DisableAntiforgery();
    }
}