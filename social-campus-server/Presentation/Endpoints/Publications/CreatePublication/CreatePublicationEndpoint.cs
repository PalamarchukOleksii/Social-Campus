using Application.Dtos;
using Application.Publications.Commands.CreatePublication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Consts;

namespace Presentation.Endpoints.Publications.CreatePublication;

public class CreatePublicationEndpoint : BaseEndpoint, IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("publications/create", async (ISender sender, [FromForm] CreatePublicationRequest request) =>
            {
                FileUploadDto? fileDto = null;
                if (request.ImageData is not null)
                    fileDto = new FileUploadDto
                    {
                        Content = request.ImageData.OpenReadStream(),
                        FileName = request.ImageData.FileName,
                        ContentType = request.ImageData.ContentType
                    };

                CreatePublicationCommand commandRequest =
                    new(request.Description, request.CreatorId, fileDto);

                var response = await sender.Send(commandRequest);

                return response.IsSuccess ? Results.Ok() : HandleFailure(response);
            })
            .Accepts<CreatePublicationRequest>("multipart/form-data")
            .WithTags(EndpointTags.Publications)
            .RequireAuthorization()
            .DisableAntiforgery();
    }
}