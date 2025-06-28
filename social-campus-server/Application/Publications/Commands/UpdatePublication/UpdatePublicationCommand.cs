using Application.Abstractions.Messaging;
using Application.Dtos;
using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.Publications.Commands.UpdatePublication;

public record UpdatePublicationCommand(
    UserId CallerId,
    PublicationId PublicationId,
    string Description,
    FileUploadDto? ImageData) : ICommand;